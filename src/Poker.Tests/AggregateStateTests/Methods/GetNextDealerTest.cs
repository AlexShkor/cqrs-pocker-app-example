using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;

namespace Poker.Tests.AggregateStateTests.Methods
{
    [TestFixture]
    public class GetNextDealerTest
    {
        [Test]
        public void GetIfDealersSet()
        {
            var state = new GameTableState();
            state.Players = new Dictionary<int, GamePlayer>();
            state.Players.Add(1, new GamePlayer
            {
                Position = 1,
                UserId = "me1"
            });
            state.Players.Add(2, new GamePlayer
            {
                Position = 2,
                UserId = "me2"
            });
            state.Dealer = 1;
            var nextPlayer = state.GetNextDealer();
            Assert.AreEqual(2, nextPlayer);
        }

        [Test]
        public void GetFirsPositionIfNotSet()
        {
            var state = new GameTableState();
            state.Players = new Dictionary<int, GamePlayer>();
            state.Players.Add(7, new GamePlayer
            {
                Position = 7,
                UserId = "me7"
            });
            state.Players.Add(3, new GamePlayer
            {
                Position = 3,
                UserId = "me3"
            });
            var nextPlayer = state.GetNextDealer();
            Assert.AreEqual(3, nextPlayer);
        }
    }
}