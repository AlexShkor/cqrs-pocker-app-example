using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Platform.Domain.Interfaces;
using Poker.Tests.Extenssions;

namespace Poker.Tests.AggregateActionsTest.Fold
{
    public class NoRaises3Players: GameTableTest
    {
        public override void Given(GameTableAggregate a)
        {
            a.CreateTable("123", "table", 100, 2);
            a.Join3Players();
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
            yield return new NextPlayerTurned
            {
                Id = "123",
                Player = new PlayerInfo
                {
                    UserId = "me3",
                    Position = 3
                }
            };
        }

        [Test]
        public override void Test()
        {
            ValidateEvents("GameId");
        }
    }
}