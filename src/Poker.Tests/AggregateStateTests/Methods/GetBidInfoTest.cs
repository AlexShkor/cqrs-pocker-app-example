using System;
using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;

namespace Poker.Tests.AggregateStateTests.Methods
{
    [TestFixture]
    public class GetBidInfoTest
    {
        private GameTableState _state;

        [SetUp]
        public void SetUp()
        {
            _state = new GameTableState();
            _state.JoinedPlayers = new Dictionary<string, TablePlayer>();
            _state.Players = new Dictionary<int, GamePlayer>();
            _state.JoinedPlayers.Add("me1", new TablePlayer
            {
                UserId = "me1",
                Position = 1,
                Cash = 98
            });
            _state.Players.Add(1, new GamePlayer
            {
                Bid = 2,
                UserId = "me1",
                Position = 1,
                AllIn = false,
                Fold = false
            });
            _state.CurrentBidding = new BiddingInfo(1);
        }

        [Test]
        public void GetsBidCorrectly()
        {
            var bid = _state.GetBidInfo(1, 10, BidTypeEnum.Call);
            Assert.AreEqual(1, bid.Position);
            Assert.AreEqual(88, bid.NewCashValue);
            Assert.AreEqual(10, bid.Odds);
            Assert.AreEqual(12, bid.Bid);
            Assert.AreEqual("me1", bid.UserId);
            Assert.AreEqual(false, bid.IsAllIn());
        }

        [Test]
        public void IfNotEnoughtCash()
        {
            _state.JoinedPlayers["me1"].Cash = 6;
            Assert.Throws<InvalidOperationException>(() => _state.GetBidInfo(1, 10, BidTypeEnum.Call));
        }

        [Test]
        public void IfNoSuchPlayer()
        {
            _state.Players.Remove(1);
            Assert.Throws<KeyNotFoundException>(() => _state.GetBidInfo(1, 10, BidTypeEnum.Call));
        }

        [Test]
        public void IfNoSuchJoinedPlayer()
        {
            _state.JoinedPlayers.Remove("me1");
            Assert.Throws<KeyNotFoundException>(() => _state.GetBidInfo(1, 10, BidTypeEnum.Call));
        }
    }
}