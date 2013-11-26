using System.Collections.Generic;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Tests.AggregateActionsTest.JoinPlayer
{
    public class JoinPlayerSuccessful : GameTableTest
    {
        public override void Given(GameTableAggregate a)
        {
            a.CreateTable("123", "table", 100, 2);
        }

        public override void When(GameTableAggregate a)
        {
            a.JoinTable("me1", 100);
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new PlayerJoined
            {
                Id = "123",
                Cash = 100,
                Position = 1,
                UserId = "me1"
            };
        }
    }
}