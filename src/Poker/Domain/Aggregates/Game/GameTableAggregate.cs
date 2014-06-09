﻿using System;
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
                        var winners = detector.GetWinners(bank).GroupBy(x => x.PokerHand.Score).OrderByDescending(x => x.Key);
                        var winnersInfo = new List<WinnerInfo>();
                        var order = 1;
                        foreach (var winnersGroup in winners)
                        {
                            foreach (var winner in winnersGroup)
                            {
                                winnersInfo.Add(new WinnerInfo(winner.UserId, State.JoinedPlayers[winner.UserId].Position, State.GetPrize(winner.UserId), order, winnersGroup.Key));
                            }
                            order++;
                        }
                        Apply(new GameFinished
                        {
                            Id = State.TableId,
                            Winners = winnersInfo,
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
            //need to check did big blind said last bid
            return State.Players.Values.All(x => x.Fold || x.Bid == State.MaxBid || x.AllIn) && State.CurrentPlayer == State.GetBigBlindPlayer();
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
                    BidType = BidTypeEnum.Call,
                    Bid = State.GetBidInfo(player.Position, bid)
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
                    BidType = BidTypeEnum.Raise,
                    Bid = State.GetBidInfo(player.Position, amount)
                });
                NextTurn(player.Position);
            }
        }

        public void Fold(string userId)
        {
            IsCurrentPlayer(userId);
            var user = State.JoinedPlayers[userId];
            Apply(new PlayerFoldBid
            {
                Id = State.TableId,
                GameId = State.GameId,
                UserId = userId,
                Position = user.Position
            });
            NextTurn(user.Position);
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