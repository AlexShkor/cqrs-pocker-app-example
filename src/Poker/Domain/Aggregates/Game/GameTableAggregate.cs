using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Domain.ApplicationServices;
using Poker.Domain.Data;
using Poker.Platform.Domain;
using Poker.Platform.Extensions;

namespace Poker.Domain.Aggregates.Game
{
    public class GameTableAggregate : Aggregate<GameTableState>
    {
        public void CreateTable(string id, string name, long buyIn, long smallBlind)
        {
            if (State.TableId.HasValue())
            {
                throw new InvalidOperationException("Table is already created.");
            }
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
                Players = State.CopyPlayers(),
                Cards = pack.GetAllCards()
            });
            DealCards(id);
            SetDealerAndBlind();
        }

        private void SetDealerAndBlind()
        {
            var dealer = State.GetNextDealer();
            var smallBlindPosition = State.GetNextPlayer(dealer);
            var bigBlindPosition = State.GetNextPlayer(smallBlindPosition);
            Apply(new DealerAssigned
            {
                Id = State.TableId,
                GameId = State.GameId,
                Dealer = State.GetPlayerInfo(dealer),
                SmallBlind = State.GetPlayerInfo(smallBlindPosition),
                BigBlind = State.GetPlayerInfo(bigBlindPosition),
            });
            Apply(new BidMade
            {
                Id = State.TableId,
                GameId = State.GameId,
                Bid = State.GetBidInfo(smallBlindPosition, State.SmallBlind, BidTypeEnum.SmallBlind),
            });
            Apply(new BidMade
            {
                Id = State.TableId,
                GameId = State.GameId,
                Bid = State.GetBidInfo(bigBlindPosition, State.BigBlind, BidTypeEnum.BigBlind),
            });
            NextTurn(bigBlindPosition);
        }

        private void NextTurn(int currentPosition)
        {
            if (State.IsAllExceptOneAreFold())
            {
                var winner = State.Players.Values.Single(x => !x.Fold);
                Apply(new GameFinished
                {
                    Id = State.TableId,
                    GameId = State.GameId,
                    Winners = new List<WinnerInfo> {new WinnerInfo(winner, State.CurrentBidding.GetBank())},
                });
            }
            else
            {
                if (CheckBiddingFinished())
                {
                    if (State.Deck.Count == 5)
                    {
                        var detector = new WinnerDetector();
                        foreach (var player in State.Players.Values)
                        {
                            var cards = new List<Card>(player.Cards);
                            cards.AddRange(State.Deck);
                            detector.AddPlayer(player.UserId, cards);
                        }
                        var bank = State.CurrentBidding.GetBank();
                        var winners = detector.GetWinners(bank);
                        Apply(new GameFinished
                        {
                            Id = State.TableId,
                            Winners = winners.Select(
                                winner =>
                                    new WinnerInfo(winner.UserId, State.JoinedPlayers[winner.UserId].Position,
                                        winner.Prize, winner.PokerHand.Score)).ToList(),
                            GameId = State.GameId
                        });
                        CreateGame(GenerateGameId());
                        return;
                    }
                    else
                    {
                        var cards = new List<Card>
                        {
                            State.Pack.TakeRandom()
                        };
                        if (!State.Deck.Any())
                        {
                            cards.Add(State.Pack.TakeRandom());
                            cards.Add(State.Pack.TakeRandom());
                        }
                        Apply(new DeckDealed
                        {
                            Id = State.TableId,
                            GameId = State.GameId,
                            Cards = cards
                        });
                        currentPosition = State.Dealer.Value;
                    }
                }
                Apply(new NextPlayerTurned
                {
                    Id = State.TableId,
                    GameId = State.GameId,
                    Player = State.GetPlayerInfo(State.GetNextNotFoldPlayer(currentPosition))
                });
            }
        }

        private bool CheckBiddingFinished()
        {
            return State.CurrentBidding.CurrentStage.IsFinished() && State.CurrentPlayer == State.GetBigBlindPlayer();
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

        #region Bidding

        public void Check(string userId)
        {
            IsCurrentPlayer(userId);
            var user = State.JoinedPlayers[userId];
            var player = State.Players[user.Position];
            if (!player.Fold && (player.Bid == State.MaxBid || player.AllIn))
            {
                Apply(new BidMade
                {
                    Id = State.TableId,
                    GameId = State.GameId,
                    Bid = State.GetBidInfo(player.Position, player.Bid, BidTypeEnum.Check)
                });
                NextTurn(player.Position);
            }
        }

        public void Call(string userId)
        {
            IsCurrentPlayer(userId);
            var user = State.JoinedPlayers[userId];
            var player = State.Players[user.Position];
            var bid = State.MaxBid - player.Bid;
            if (!player.Fold)
            {
                Apply(new BidMade
                {
                    Id = State.TableId,
                    GameId = State.GameId,
                    Bid = State.GetBidInfo(player.Position, bid, BidTypeEnum.Call)
                });
                NextTurn(player.Position);
            }
        }

        public void Raise(string userId, long amount)
        {
            IsCurrentPlayer(userId);
            var user = State.JoinedPlayers[userId];
            var player = State.Players[user.Position];

            if (State.MaxBid >= player.Bid + amount)
                throw new InvalidOperationException("Rate must be higher than max bid while raising");

            if (!player.Fold)
            {
                Apply(new BidMade
                {
                    Id = State.TableId,
                    GameId = State.GameId,
                    Bid = State.GetBidInfo(player.Position, amount, BidTypeEnum.Raise)
                });
                NextTurn(player.Position);
            }
        }

        public void Fold(string userId)
        {
            IsCurrentPlayer(userId);
            var user = State.JoinedPlayers[userId];
            var player = State.Players[user.Position];

            if (player.Fold)
            {
               throw new InvalidOperationException("Player has already fold.");
            }
            Apply(new BidMade
            {
                Id = State.TableId,
                GameId = State.GameId,
                Bid = State.GetBidInfo(player.Position, player.Bid, BidTypeEnum.Fold)
            });
            NextTurn(player.Position);
        }

        #endregion

        private void IsCurrentPlayer(string userId)
        {
            if (State.GetPlayerInfo(State.CurrentPlayer).UserId != userId)
            {
                throw new InvalidOperationException("It is not your turn!");
            }
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