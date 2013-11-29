﻿using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Platform.Domain.Interfaces;
using Poker.Tests.Extenssions;

namespace Poker.Tests.AggregateActionsTest.Fold
{
    public class FinishBiddingNoRaises3Players : GameTableTest
    {
        public override void Given(GameTableAggregate a)
        {
            a.CreateTable("123", "table", 100, 2);
            a.Join3Players();
            a.Call("me2");
            a.Fold("me3");
        }

        public override void When(GameTableAggregate a)
        {
            a.Fold("me1");
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new PlayerFoldBid
            {
                Id = "123",
                UserId = "me1",
                Position = 1
            };
            yield return new GameFinished
            {
                Id = "123",
                Bank = 10,
                Winner = new PlayerInfo
                {
                    UserId = "me2",
                    Position = 2
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