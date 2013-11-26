using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;

namespace Poker.Tests.AggregateStateTests
{
    [TestFixture]
    public class DealerAssignedTest
    {
        [Test]
        public void SetsDealer()
        {
            var state = new GameTableState();
            state.Invoke(new DealerAssigned
            {
                Id = "123",
                Dealer = new PlayerInfo()
                {
                    UserId = "me1",
                    Position = 1
                }
            });
            Assert.AreEqual(1,state.Dealer);
        }
    }
}