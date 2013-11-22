using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Tests.AggregateActionsTest.JoinPlayer
{
    public class TableIsFullTest : AggregateTest<GameTableAggregate, GameTableState>
    {
        public override void Given(GameTableAggregate a)
        {
            a.CreateTable("123", "table", 100, 2);
            a.JoinTable("me1", 100);
            a.JoinTable("me2", 100);
            a.JoinTable("me3", 100);
            a.JoinTable("me4", 100);
            a.JoinTable("me5", 100);
            a.JoinTable("me6", 100);
            a.JoinTable("me7", 100);
            a.JoinTable("me8", 100);
            a.JoinTable("me9", 100);
            a.JoinTable("me10", 100);
        }

        public override void When(GameTableAggregate a)
        {
            Assert.Throws<InvalidOperationException>(() => a.JoinTable("me11", 100));
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield break;
        }
    }
}