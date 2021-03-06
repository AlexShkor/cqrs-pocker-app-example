﻿using System.Collections.Generic;
using Poker.Domain.Aggregates.Game;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Tests.AggregateActionsTest.Check
{
    public class PlayerCantIfBidIsNotEquelCheck : GameTableTest
    {
        public override void Given(GameTableAggregate a)
        {
            a.CreateTable("123", "table", 100, 2);
            a.JoinTable("me1", 100);
            a.JoinTable("me2", 100);
        }

        public override void When(GameTableAggregate a)
        {
            a.Check("me2");
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield break;
        }
    }
}