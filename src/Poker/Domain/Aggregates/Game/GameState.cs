using System;
using System.Collections.Generic;
using System.Linq;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Domain.Data;
using Poker.Platform.Domain;

namespace Poker.Domain.Aggregates.Game
{
    public sealed class GameTableState: AggregateState
    {

        public static readonly int[] Positions = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

        public Dictionary<string,TablePlayer> JoinedPlayers { get; set; }

        public Dictionary<int,GamePlayer> Players { get; set; }

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
                JoinedPlayers[e.Winner.UserId].Cash += e.Bank;
            });
            On((GameCreated e) =>
            {
                GameId = e.GameId;
                Pack = new Pack(e.Cards);
                Deck = new List<Card>();
                SitPlayers(e.Players);
                CurrentBidding = new BiddingInfo();
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
            On((BlindBidsMade e) =>
            {
                AddBid(e.SmallBlind);
                AddBid(e.BigBlind);
            });
            On((BidMade e) => AddBid(e.Bid));
            On((NextPlayerTurned e) =>
            {
                CurrentPlayer = e.Player.Position;
            });
            On((PlayerFoldBid e) =>
            {
                Players[e.Position].Fold = true;
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

        private void AddBid(BidInfo bid)
        {
            CurrentBidding.AddBid(bid);
            Players[bid.Position].Bid = bid.Bid;
            Players[bid.Position].AllIn = bid.AllIn;
            JoinedPlayers[bid.UserId].Cash -= bid.Odds;
            if (bid.Bid > MaxBid)
            {
                MaxBid = bid.Bid;
            }
        }

        private void SitPlayers(IEnumerable<TablePlayer> players)
        {
            Players.Clear();
            foreach (var player in players.OrderBy(x=> x.Position))
            {
                Players.Add(player.Position,new GamePlayer()
                {
                    Position = player.Position,
                    UserId = player.UserId
                });
            }
        }

        public int GetNextPlayer(int position, Func<GamePlayer,bool> predicate = null)
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
            return Players.Select(x => x.Value.Position).OrderBy(x=> x).First();
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

        public BidInfo GetBidInfo(int position, long bid)
        {
            var player = Players[position];
            var user = JoinedPlayers[player.UserId];
            if (user.Cash < bid)
            {
                throw new InvalidOperationException("Not enought cash for user {0} ");
            }
            return new BidInfo
            {
                Position = position,
                Bid = player.Bid + bid,
                Odds = bid,
                AllIn = user.Cash == bid,
                UserId = player.UserId,
                NewCashValue = user.Cash - bid,
                BiddingStage = CurrentBidding.Stage
            };
            
        }

        public List<TablePlayer> CopyPlayers()
        {
            return JoinedPlayers.Values.Select(x => new TablePlayer
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
    }

    public class BiddingInfo
    {
        public List<BiddingStage> BiddingStages { get; private set; }

        public BiddingStage CurrentStage
        {
            get { return BiddingStages.Last(); }
        }

        public int Stage
        {
            get { return BiddingStages.Count - 1; }
        }

        public BiddingInfo()
        {
            BiddingStages = new List<BiddingStage>();
            NextStage();
        }

        public void AddBid(BidInfo bid)
        {
            CurrentStage.Bids.Add(bid);
        }

        public long GetBank()
        {
            return CurrentStage.GetBank();
        }

        public void NextStage()
        {
            BiddingStages.Add(new BiddingStage());
        }
    }

    public class BiddingStage
    {
        public List<BidInfo> Bids { get; private set; }

        public BiddingStage()
        {
            Bids = new List<BidInfo>();
        }

        public long GetBank()
        {
            return Bids.GroupBy(x => x.UserId).Select(x => x.Select(b => b.Bid).Max()).Sum();
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
        public bool AllIn { get; set; }
    }

    public enum BiddingStagesEnum
    {
        PreFlop = 0,
        Flop = 1,
        Turn = 2,
        River = 3
    }
}