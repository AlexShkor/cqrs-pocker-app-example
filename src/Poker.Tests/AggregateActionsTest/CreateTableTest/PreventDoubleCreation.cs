using System;
using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Tests.AggregateActionsTest.CreateTableTest
{
    public class PreventDoubleCreation : AggregateTest<GameTableAggregate, GameTableState>
    {
        public override void Given(GameTableAggregate a)
        {
            a.CreateTable("123","table",100,2);
        }

        public override void When(GameTableAggregate a)
        {
            Assert.Throws<InvalidOperationException>(() => a.CreateTable("123", "table", 100, 2));
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield break;
        }
    }
}