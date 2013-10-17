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

        public Dictionary<string,TablePlayer> JoinedPlayers { get; private set; }

        public Dictionary<int,GamePlayer> Players { get; private set; }

        public string GameId { get; private set; }

        public string TableId { get; set; }

        public long BuyIn { get; private set; }

        public long SmallBlind { get; private set; }

        public long BigBlind { get { return SmallBlind * 2; } }

        public int CurrentPlayer { get; set; }

        public int? Dealer { get; private set; }

        public BiddingInfo CurrentBidding { get; private set; }

        public Pack Pack { get; set; }

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
            });
            On((GameCreated e) =>
            {
                GameId = e.GameId;
                Pack = new Pack(e.Cards);
                SitPlayers(e.Players);
                CurrentBidding = new BiddingInfo();
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
        }

        private void AddBid(BidInfo bid)
        {
            CurrentBidding.Bids.Add(bid);
            Players[bid.Position].Bid = bid.Bid;
            Players[bid.Position].AllIn = bid.AllIn;
            JoinedPlayers[bid.UserId].Cash -= bid.Bid;
            if (bid.Bid > MaxBid)
            {
                MaxBid = bid.Bid;
            }
        }

        private void SitPlayers(List<TablePlayer> players)
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

        public int GetNextPlayer(int position)
        {
            for (int i = 1; i < 10; i++)
            {
                var index = (position + i) % 10;
                if (Players.ContainsKey(index))
                {
                    return index;
                }

            }
            return position;
        }

        public int GetNextDealer()
        {
            if (Dealer.HasValue)
            {
                return GetNextPlayer(Dealer.Value);
            }
            return Players.Select(x => x.Value.Position).First();
        }

        public bool IsTableFull()
        {
            return JoinedPlayers.Count >= MaxPlayers;
        }

        public bool IsPositionTaken(int position)
        {
            return JoinedPlayers.Values.Any(x => x.Position == position);
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
            if (user.Cash >= bid)
            {
                return new BidInfo
                {
                    Position = position,
                    Bid = player.Bid + bid,
                    Odds = bid,
                    AllIn = user.Cash == bid,
                    UserId = player.UserId,
                    NewCashValue = user.Cash - bid
                };
            }
            throw new InvalidOperationException("Not enought cash for user {0} ");
        }
    }

    public class BiddingInfo
    {
        public List<BidInfo> Bids { get; set; }

        public BiddingInfo()
        {
            Bids = new List<BidInfo>();
        }
    }

    public class BidInfo
    {
        public string UserId { get; set; }
        public int Position { get; set; }
        public long Bid { get; set; }
        public long NewCashValue { get; set; }
        public long Odds { get; set; }
        public bool AllIn { get; set; }
    }
}