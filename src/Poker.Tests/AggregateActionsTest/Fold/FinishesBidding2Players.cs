using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Tests.AggregateActionsTest.Fold
{
    public class FinishesBidding2Players: GameTableTest
    {
        public override void Given(GameTableAggregate a)
        {
            a.CreateTable("123", "table", 100, 2);
            a.JoinTable("me1", 100);
            a.JoinTable("me2", 100);
        }

        public override void When(GameTableAggregate a)
        {
            a.Fold("me2");
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new PlayerFoldBid
            {
                Id = "123",
                UserId = "me2",
                Position = 2
            };
            yield return new GameFinished
            {
                Id = "123",
                Winners = new List<WinnerInfo> { new WinnerInfo("me1", 1,6,1)}
            };
        }

        [Test]
        public override void Test()
        {
            ValidateEvents("GameId");
        }
    }
}