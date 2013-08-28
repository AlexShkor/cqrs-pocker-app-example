using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AKQ.Domain;
using PAQK.Domain.Aggregates.Game.Data;
using PAQK.Domain.Aggregates.Game.Events;
using PAQK.Platform.Domain;

namespace PAQK.Domain.Aggregates.Game
{
    public sealed class GameTableState: AggregateState
    {
        public Dictionary<string,TablePlayer> JoinedPlayers { get; private set; }

        public Dictionary<int,GamePlayer> Players { get; private set; }

        public string GameId { get; private set; }

        public string TableId { get; set; }

        public long BuyIn { get; private set; }

        public long SmallBlind { get; private set; }

        public long BigBlind { get { return SmallBlind * 2; } }

        public int CurrentPlayer { get; set; }

        public int Dealer { get; private set; }

        public long CurrentBid { get; private set; }

        public BiddingInfo CurrentBidding { get; private set; }

        public int GetNextPlayer(int position)
        {
            for (int i = 1; i < 10; i++)
            {
                var index = (position + i)%10;
                if (Players.ContainsKey(position))
                {
                    return index;
                }

            }
            return position;
        }

        public Pack Pack { get; set; }

        public readonly int MaxPlayers = 10;

        public GameTableState()
        {
            On((TableCreated e) =>
            {
                TableId = e.Id;
                SmallBlind = e.SmallBlind;
                BuyIn = e.BuyIn;
                Dealer = 0;
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
                JoinedPlayers[e.SmallBlind.UserId].Cash -= e.SmallBlind.Bid;
                AddBid(e.SmallBlind);
                JoinedPlayers[e.BigBlind.UserId].Cash -= e.BigBlind.Bid;
                AddBid(e.BigBlind);
            });
        }

        private void AddBid(BidInfo bid)
        {
            CurrentBidding.Bids.Add(bid);
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

        public string GetNextDealer()
        {
            throw new System.NotImplementedException();
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
                    Bid = bid,
                    UserId = player.UserId
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
    }
}