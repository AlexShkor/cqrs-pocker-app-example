using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Tests.AggregateActionsTest.Raise
{
    public class RateMustBeGreaterThanMaxBidByBigBlind : GameTableTest
    {
        public override void Given(GameTableAggregate a)
        {
            a.CreateTable("123", "table", 100, 5);
            a.JoinTable("me1", 100);
            a.JoinTable("me2", 100);

            a.Raise("me2", 15);
            a.Raise("me1", 20);
        }

        public override void When(GameTableAggregate a)
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                a.Raise("me2", 10);
            });
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield break;
        }

        [Test]
        public override void Test()
        {
            base.Test();
        }
    }
}
