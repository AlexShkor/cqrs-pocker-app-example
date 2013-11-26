using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;

namespace Poker.Tests.AggregateStateTests.Methods
{
    [TestFixture]
    public class GetNextPlayerTest
    {
        [Test]
        public void GetsNextPlayerInSiquant2()
        {
            var state = new GameTableState();
            state.Players = new Dictionary<int, GamePlayer>();
            state.Players.Add(1, new GamePlayer
            {
                Position = 1,UserId = "me1"
            });
            state.Players.Add(2, new GamePlayer
            {
                Position = 2,UserId = "me2"
            });
            var nextPlayer = state.GetNextPlayer(1);
            Assert.AreEqual(2,nextPlayer);
        }

        [Test]
        public void GetsNextPlayerNotSiquant2()
        {
            var state = new GameTableState();
            state.Players = new Dictionary<int, GamePlayer>();
            state.Players.Add(2, new GamePlayer
            {
                Position = 2,UserId = "me2"
            });
            state.Players.Add(5, new GamePlayer
            {
                Position = 5,UserId = "me5"
            });
            var nextPlayer = state.GetNextPlayer(2);
            Assert.AreEqual(5,nextPlayer);
        }

        [Test]
        public void GetsCircledNextPlayerNotSiquant2()
        {
            var state = new GameTableState();
            state.Players = new Dictionary<int, GamePlayer>();
            state.Players.Add(2, new GamePlayer
            {
                Position = 2,
                UserId = "me2"
            });
            state.Players.Add(5, new GamePlayer
            {
                Position = 5,
                UserId = "me5"
            });
            var nextPlayer = state.GetNextPlayer(5);
            Assert.AreEqual(2, nextPlayer);
        }

        [Test]
        public void GetsNextPlayerNotSiquant4()
        {
            var state = new GameTableState();
            state.Players = new Dictionary<int, GamePlayer>();
            state.Players.Add(2, new GamePlayer
            {
                Position = 2,UserId = "me2"
            });
            state.Players.Add(3, new GamePlayer
            {
                Position = 3,UserId = "me3"
            });
            state.Players.Add(5, new GamePlayer
            {
                Position = 5,UserId = "me5"
            });
            state.Players.Add(8, new GamePlayer
            {
                Position = 8,UserId = "me8"
            });
            var nextPlayer = state.GetNextPlayer(5);
            Assert.AreEqual(8,nextPlayer);
        }

        [Test]
        public void GetsNextWithFoldPredicate()
        {
            var state = new GameTableState();
            state.Players = new Dictionary<int, GamePlayer>();
            state.Players.Add(2, new GamePlayer
            {
                Position = 2,
                UserId = "me2"
            });
            state.Players.Add(3, new GamePlayer
            {
                Position = 3,
                UserId = "me3",
                Fold = true
            });
            state.Players.Add(5, new GamePlayer
            {
                Position = 5,
                UserId = "me5",
                Fold = true
            });
            state.Players.Add(8, new GamePlayer
            {
                Position = 8,
                UserId = "me8"
            });
            var nextPlayer = state.GetNextPlayer(8, player => player.Fold);
            Assert.AreEqual(3, nextPlayer);
        }
    }
}