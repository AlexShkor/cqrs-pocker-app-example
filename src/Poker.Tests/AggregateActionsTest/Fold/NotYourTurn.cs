using System;
using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Tests.AggregateActionsTest.Fold
{
    public class NotYourTurn : GameTableTest
    {
        public override void Given(GameTableAggregate a)
        {
            a.CreateTable("123", "table", 100, 2);
            a.JoinTable("me1", 100);
            a.JoinTable("me2", 100);
        }

        public override void When(GameTableAggregate a)
        {
            Assert.Throws<InvalidOperationException>(() => a.Fold("me1"));
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield break;
        }
    }
}