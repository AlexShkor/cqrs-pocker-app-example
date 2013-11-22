using System.Collections.Generic;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Tests.AggregateActionsTest.JoinPlayer
{
    public class DontCreateGameIfItIsStartedTest : AggregateTest<GameTableAggregate, GameTableState>
    {
        public override void Given(GameTableAggregate a)
        {
            a.CreateTable("123", "table", 100, 2);
            a.JoinTable("me1", 100);
            a.JoinTable("me2", 100);
        }

        public override void When(GameTableAggregate a)
        {
            a.JoinTable("me3", 200);
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new PlayerJoined
            {
                Id = "123",
                Cash = 200,
                Position = 3,
                UserId = "me3"
            };
        }
    }
}