using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;

namespace Poker.Tests.AggregateStateTests.Methods
{
    [TestFixture]
    public class IsAllExceptOneAreFold
    {
        [Test]
        public void ReturnsTrue()
        {
            var state = new GameTableState();
            state.Players = new Dictionary<int, GamePlayer>();
            for (int i = 1; i <= 5; i++)
            {
                state.Players.Add(i, new GamePlayer
                {
                    Position = i,
                    UserId = "me" + i,
                    Fold = true
                });
            }
            state.Players.Add(10,new GamePlayer() {UserId = "me10", Position = 10});
            var result = state.IsAllExceptOneAreFold();
            Assert.IsTrue(result);
        } 
        
        
        [Test]
        public void ReturnsFalse()
        {
            var state = new GameTableState();
            state.Players = new Dictionary<int, GamePlayer>();
            for (int i = 1; i <= 5; i++)
            {
                state.Players.Add(i, new GamePlayer
                {
                    Position = i,
                    UserId = "me" + i,
                    Fold = true
                });
            }
            state.Players.Add(9,new GamePlayer() {UserId = "me9", Position = 9});
            state.Players.Add(10,new GamePlayer() {UserId = "me10", Position = 10});
            var result = state.IsAllExceptOneAreFold();
            Assert.IsFalse(result);
        }
    }
}