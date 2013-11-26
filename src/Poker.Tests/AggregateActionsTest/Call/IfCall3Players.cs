using System;
using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Platform.Domain.Interfaces;
using Poker.Tests.Extenssions;

namespace Poker.Tests.AggregateActionsTest.Call
{
    public class IfCall3Players : GameTableTest
    {
        public override void Given(GameTableAggregate a)
        {
            a.CreateTable("123", "table", 100, 2);
            a.Join3Players();
        }

        public override void When(GameTableAggregate a)
        {
           a.Call("me2");
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new BidMade
            {
                Id = "123",
                BidType = BidTypeEnum.Call,
                Bid = new BidInfo
                {
                    UserId = "me2",
                    Position = 2,
                    Odds = 4,
                    Bid = 4,
                    AllIn = false,
                    NewCashValue = 96
                }
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