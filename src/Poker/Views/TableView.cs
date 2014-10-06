using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Data;
using Uniform;

namespace Poker.Views
{
    public class TableView
    {
        [BsonId, DocumentId]
        public string Id { get; set; }

        public string Name { get; set; }

        public long BuyIn { get; set; }

        public long SmallBlind { get; set; }
        public int MaxPlayers { get; set; }
        public string CurrentGameId { get; set; }

        public List<Card> Deck { get; set; }

        public List<PlayerDocument> Players { get; set; }

        public string CurrentPlayerId { get; set; }
        public long Bank { get; set; }
        public long MinBet { get; set; }

        public TableView()
        {
            Deck = new List<Card>();
            Players = new List<PlayerDocument>();
        }

        public void AddPlayerCard(string userId, Card card)
        {
            var player = Players.Find(x => x.UserId == userId);
            player.Cards.Add(card);
        }

        public void SetBid(string userId, BidInfo bidInfo)
        {
            var player = GetPlayer(userId);
            player.Bid = bidInfo.Bid;
            player.Cash = bidInfo.NewCashValue;
            player.Bet = bidInfo.Bet;
            MarkBlindsIfGameStarts(player, bidInfo.BidType);
        }

        private void MarkBlindsIfGameStarts(PlayerDocument player, BidTypeEnum bidType)
        {
            if (bidType == BidTypeEnum.SmallBlind || bidType == BidTypeEnum.BigBlind)
            {
                player.IsSmallBlind = bidType == BidTypeEnum.SmallBlind;
                player.IsBigBlind = bidType == BidTypeEnum.BigBlind;
            }
        }

        public PlayerDocument GetPlayer(string userId)
        {
            return Players.Find(x => x.UserId == userId);
        }
    }
}