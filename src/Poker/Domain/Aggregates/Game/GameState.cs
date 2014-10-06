using System;
using System.Collections.Generic;
using System.Linq;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Domain.Data;
using Poker.Platform.Domain;

namespace Poker.Domain.Aggregates.Game
{
    public sealed class GameTableState : AggregateState
    {
        public static readonly int[] Positions = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        public Dictionary<string, TablePlayer> JoinedPlayers { get; set; }

        public Dictionary<int, GamePlayer> Players { get; set; }

        public string GameId { get; private set; }

        public string TableId { get; set; }

        public long BuyIn { get; private set; }

        public long SmallBlind { get; private set; }

        public long BigBlind { get { return SmallBlind * 2; } }

        public int CurrentPlayer { get; set; }

        public int? Dealer { get; set; }

        public BiddingInfo CurrentBidding { get; set; }

        public Pack Pack { get; set; }

        public List<Card> Deck { get; set; }

        public long MaxBid { get; set; }

        public long MaxRaise { get; set; }

        public readonly int MaxPlayers = 10;

        public GameTableState()
        {
            On((TableCreated e) =>
            {
                TableId = e.Id;
                SmallBlind = e.SmallBlind;
                BuyIn = e.BuyIn;
                Dealer = null;
                JoinedPlayers = new Dictionary<string, TablePlayer>();
                Players = new Dictionary<int, GamePlayer>();
            });
            On((GameFinished e) =>
            {
                GameId = null;
                MaxBid = 0;
                CurrentBidding = null;

                foreach (var winner in e.Winners)
                {
                    JoinedPlayers[winner.UserId].Cash += winner.Amount;
                }
            });

            On((GameCreated e) =>
            {
                GameId = e.GameId;
                Pack = new Pack(e.Cards);
                Deck = new List<Card>();
                SitPlayers(e.Players);
                CurrentBidding = new BiddingInfo(e.Players.Count);
                MaxRaise = SmallBlind;
            });
            On((DealerAssigned e) =>
            {
                Dealer = e.Dealer.Position;
            });
            On((PlayerJoined e) => JoinedPlayers.Add(e.UserId, new TablePlayer()
            {
                Cash = e.Cash,
                Position = e.Position,
                UserId = e.UserId,
            }));
            On((CardsDealed e) =>
            {
                foreach (var playerCard in e.Cards)
                {
                    Pack.Remove(playerCard.Card);
                    Players[playerCard.Position].Cards.Add(playerCard.Card);
                }
            });
            On((BidMade e) => AddBidEvent(e.Bid));
            On((BiddingFinished e) =>
            {
                MaxRaise = SmallBlind;
            });
            On((NextPlayerTurned e) =>
            {
                CurrentPlayer = e.Player.Position;
                MaxRaise = e.MaxRaise;
            });
            On((DeckDealed e) =>
            {
                foreach (var card in e.Cards)
                {
                    Pack.Remove(card);
                    Deck.Add(card);
                }
                CurrentBidding.NextStage();
            });
        }

        private void AddBidEvent(BidInfo bid)
        {
            CurrentBidding.AddBid(bid);
            Players[bid.Position].Bid = bid.Bid;
            Players[bid.Position].AllIn = bid.IsAllIn();
            Players[bid.Position].Fold = bid.IsFold();
            JoinedPlayers[bid.UserId].Cash -= bid.Odds;
            if (bid.Bid > MaxBid)
            {
                MaxBid = bid.Bid;
            }
        }

        private void SitPlayers(IEnumerable<TablePlayer> players)
        {
            Players.Clear();
            foreach (var player in players.OrderBy(x => x.Position))
            {
                Players.Add(player.Position, new GamePlayer()
                {
                    Position = player.Position,
                    UserId = player.UserId
                });
            }
        }

        public int GetNextPlayer(int position, Func<GamePlayer, bool> predicate = null)
        {
            for (int i = 1; i < 10; i++)
            {
                var index = (position + i) % 10;
                if (Players.ContainsKey(index) && (predicate == null || predicate(Players[index])))
                {
                    return index;
                }
            }
            return position;
        }

        public int GetNextNotFoldPlayer(int position)
        {
            return GetNextPlayer(position, player => !player.Fold);
        }

        public int GetNextDealer()
        {
            if (Dealer.HasValue)
            {
                return GetNextPlayer(Dealer.Value);
            }
            return Players.Select(x => x.Value.Position).OrderBy(x => x).First();
        }

        public bool IsTableFull()
        {
            return JoinedPlayers.Count >= MaxPlayers;
        }

        public bool HasUser(string userId)
        {
            return JoinedPlayers.ContainsKey(userId);
        }

        public PlayerInfo GetPlayerInfo(int position)
        {
            return new PlayerInfo(Players[position]);
        }

        public PlayerInfo GetWinnerInfo(int position)
        {
            return new PlayerInfo(Players[position]);
        }

        public BidInfo GetBidInfo(int position, long amount, BidTypeEnum bidType)
        {
            var player = Players[position];
            var user = JoinedPlayers[player.UserId];
            if (user.Cash < amount)
            {
                throw new InvalidOperationException("Not enought cash for user {0} ");
            }
            var bet = CurrentBidding.CurrentStage.GetBetForPlayer(position) + amount;
            if (bidType == BidTypeEnum.Raise)
            {
                MaxRaise = Math.Max(MaxRaise, amount - MaxRaise);
            }
            return new BidInfo
            {
                Position = position,
                Bid = player.Bid + amount,
                Odds = amount,
                Bet = bet,
                UserId = player.UserId,
                NewCashValue = user.Cash - amount,
                BiddingStage = CurrentBidding.Stage,
                BidType = bidType
            };
        }

        public List<TablePlayer> CopyPlayers()
        {
            return JoinedPlayers.Values.OrderBy(x => x.Position).Select(x => new TablePlayer
            {
                UserId = x.UserId,
                Position = x.Position,
                Cash = x.Cash
            }).ToList();
        }

        public int GetBigBlindPlayer()
        {
            return GetNextPlayer(GetNextPlayer(Dealer.Value));
        }

        public bool IsAllExceptOneAreFold()
        {
            return Players.Values.Count(x => x.Fold) == Players.Count - 1;
        }

        public long GetMinBet()
        {
            var maxBet = CurrentBidding.CurrentStage.GetMaxBet();
            if (maxBet == 0) //Pre-Flop only
            {
                return BigBlind;
            }
            return maxBet + MaxRaise;
        }
    }

    public class BidInfo
    {
        public string UserId { get; set; }
        public int Position { get; set; }
        public int BiddingStage { get; set; }
        public long Bid { get; set; }
        public long NewCashValue { get; set; }
        public long Odds { get; set; }
        public BidTypeEnum BidType { get; set; }
        public long Bet { get; set; }

        public bool IsAllIn()
        {
            return NewCashValue == 0;
        }

        public bool IsFold()
        {
            return BidType == BidTypeEnum.Fold;
        }
    }

    public enum BidTypeEnum
    {
        Check = 0,
        Call = 1,
        Raise = 2,
        Fold = 3,
        SmallBlind = 4,
        BigBlind = 5
    }

    public enum BiddingStagesEnum
    {
        PreFlop = 0,
        Flop = 1,
        Turn = 2,
        River = 3
    }
}