using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Events;

namespace Poker.Tests.AggregateStateTests
{
    [TestFixture]
    public class TableCreatedStateTest
    {
        [Test]
        public void TableCreated()
        {
            var state = new GameTableState();
            state.Invoke(new TableCreated
            {
                Id = "1",
                BuyIn = 1000,
                MaxPlayers = 10,
                Name = "name",
                SmallBlind = 5
            });
            Assert.AreEqual("1", state.TableId);
            Assert.AreEqual(1000, state.BuyIn);
            Assert.AreEqual(10, state.MaxPlayers);
            Assert.AreEqual(5, state.SmallBlind);
            Assert.AreEqual(10, state.BigBlind);
            Assert.IsNull(state.Dealer);
            Assert.IsNull(state.CurrentBidding);
            Assert.IsEmpty(state.JoinedPlayers);
            Assert.IsEmpty(state.Players);
            Assert.AreEqual(0, state.MaxBid);
            Assert.AreEqual(0, state.CurrentPlayer);
        } 
    }
}