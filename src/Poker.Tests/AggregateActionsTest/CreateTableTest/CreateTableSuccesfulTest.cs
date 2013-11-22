using System.Collections.Generic;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Tests.AggregateActionsTest.CreateTableTest
{
    public class CreateTableSuccesfulTest : AggregateTest<GameTableAggregate, GameTableState>
    {
        public override void Given(GameTableAggregate a)
        {
            return;
        }

        public override void When( GameTableAggregate a)
        {
            a.CreateTable("1", "table", 100, 2);
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new TableCreated
            {
                Id = "1",
                Name = "table",
                BuyIn = 100,
                MaxPlayers = 10,
                SmallBlind = 2
            };
        }
    }
}