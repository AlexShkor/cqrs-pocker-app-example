﻿using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Domain.Data;
using Poker.Platform.Domain;
using Poker.Platform.Extensions;

namespace Poker.Domain.Aggregates.Game
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
                BigBlind = State.GetBidInfo(bigBlind, State.BigBlind),
            });
            NextTurn(bigBlind);
        }

        private void NextTurn(int currentPosition)
        {
            if (CheckBiddingFinished())
            {
                
            }
            else
            {
                Apply(new NextPlayerTurned
                {
                    Id = State.TableId,
                    GameId = State.GameId,
                    Player = State.GetPlayerInfo(State.GetNextPlayer(currentPosition))
                });
            }
        }

        private bool CheckBiddingFinished()
        {
            //need to check did big blind said last bid
            return State.Players.Values.All(x => x.Fold || x.Bid == State.MaxBid || x.AllIn);
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
            var user = State.JoinedPlayers[userId];
            var player = State.Players[user.Position];
            if (!player.Fold  && (player.Bid == State.MaxBid || player.AllIn))
            {
                Apply(new PlayerCheckedBid
                {
                    Id = State.TableId,
                    GameId = State.GameId,
                    UserId = userId,
                    Position = player.Position
                });
                NextTurn(player.Position);
            }
        }

        public void Call(string userId)
        {
            var user = State.JoinedPlayers[userId];
            var player = State.Players[user.Position];
            var bid = State.MaxBid - player.Bid;
            if (!player.Fold)
            {
                Apply(new BidMade
                {
                    Id = State.TableId,
                    GameId = State.GameId,
                    BidType = BidTypeEnum.Call,
                    Bid = State.GetBidInfo(player.Position,bid)
                });
                NextTurn(player.Position);
            }
        }

        public void Raise(string userId, long amount)
        {
            var user = State.JoinedPlayers[userId];
            var player = State.Players[user.Position];
            if (!player.Fold)
            {
                Apply(new BidMade
                {
                    Id = State.TableId,
                    GameId = State.GameId,
                    BidType = BidTypeEnum.Raise,
                    Bid = State.GetBidInfo(player.Position, amount)
                });
                NextTurn(player.Position);
            }
        }

        public void Fold(string userId)
        {
            var user = State.JoinedPlayers[userId];
            Apply(new PlayerFoldBid
            {
                Id = State.TableId,
                GameId = State.GameId,
                UserId = userId,
                Position = user.Position
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