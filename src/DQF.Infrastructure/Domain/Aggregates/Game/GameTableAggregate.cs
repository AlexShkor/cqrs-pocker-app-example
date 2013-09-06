using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using PAQK.Domain.Aggregates.Game.Data;
using PAQK.Domain.Aggregates.Game.Events;
using PAQK.Domain.Aggregates.Site;
using PAQK.Domain.Data;
using PAQK.Platform.Domain;
using PAQK.Platform.Domain.Interfaces;
using PAQK.Platform.Extensions;

namespace PAQK.Domain.Aggregates.Game
{
    public class GameTableAggregate : Aggregate<GameTableState>
    {

        public void CreateTable(string id, string name, long buyIn, long smallBlind)
        {
            Apply(new TableCreated()
            {
                Id = id,
                Name = name,
                BuyIn = buyIn,
                SmallBlind = smallBlind,
                MaxPlayers = State.MaxPlayers
            });
        }

        public void CreateGame(string id)
        {
            if (State.GameId.HasValue())
            {
                throw new InvalidOperationException("Game can't be created on this table while previous game isn't finished.");
            }
            if (State.JoinedPlayers.Count < 2)
            {
                throw new InvalidOperationException("Not enough players to start the game.");
            }
            var pack = new Pack();
            Apply(new GameCreated()
            {
                Id = State.TableId,
                GameId = id,
                Players = State.JoinedPlayers.Values.ToList(),
                Cards = pack.GetAllCards()
            });
            DealCards(id);
            SetDealerAndBlind();
        }

        private void SetDealerAndBlind()
        {
            var dealer = State.GetNextDealer();
            var smallBlind = State.GetNextPlayer(dealer);
            var bigBlind = State.GetNextPlayer(smallBlind);
            Apply(new DealerAssigned
            {
                Id = State.TableId,
                GameId = State.GameId,
                Dealer = State.GetPlayerInfo(dealer),
                SmallBlind = State.GetPlayerInfo(smallBlind),
                BigBlind = State.GetPlayerInfo(bigBlind),
            });
            Apply(new BlindBidsMade
            {
                Id = State.TableId,
                GameId = State.GameId,
                SmallBlind = State.GetBidInfo(smallBlind, State.SmallBlind),
                BigBlind = State.GetBidInfo(smallBlind, State.BigBlind),
            });
        }

        public void FinishGame(string id)
        {
            Apply(new GameFinished()
            {
                Id = id,
            });
        }

        public void JoinTable(string userId, long cashInCents)
        {
            if (State.IsTableFull())
            {
                throw new InvalidOperationException("Table is full.");
            }
            var position = GameTableState.Positions.Except(State.JoinedPlayers.Select(x => x.Value.Position)).First();
            if (State.IsPositionTaken(position))
            {
                throw new InvalidOperationException("Position is already taken.");
            }
            if (!State.HasUser(userId))
            {
                Apply(new PlayerJoined
                {
                    Position = position,
                    Id = State.TableId,
                    UserId = userId,
                    Cash = cashInCents
                });
                if (State.GameId == null && State.JoinedPlayers.Count >= 2)
                {
                    CreateGame(GenerateGameId());
                }
            }
        }

        private static string GenerateGameId()
        {
            return ObjectId.GenerateNewId().ToString();
        }

        private void DealCards(string gameId)
        {
            var takenCards = new List<PlayerCard>();

            foreach (var place in State.Players.Values)
            {
                var cards = State.Pack.TakeFew(2);
                foreach (var card in cards)
                {
                    takenCards.Add(new PlayerCard()
                    {
                        Position = place.Position,
                        UserId = place.UserId,
                        Card = card
                    });
                }
            }
            Apply(new CardsDealed
            {
                Id = State.TableId,
                GameId = gameId,
                Cards = takenCards
            });
        }

        public void Check(string userId)
        {
        }

        public void Call(string userId)
        {
            
        }

        public void Raise(string userId, long amount)
        {
            
        }

        public void Fold(string userId)
        {
            Apply(new PlayerFoldBid
            {
                Id = State.TableId,
                GameId = State.GameId,
                UserId = userId
            });
        }

        public void Archive()
        {
            Apply(new TableArchived
            {
                Id = State.TableId
            });
        }
    }
}