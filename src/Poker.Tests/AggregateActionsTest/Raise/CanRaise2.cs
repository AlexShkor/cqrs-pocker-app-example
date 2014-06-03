using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Tests.AggregateActionsTest.Raise
{
    public class CanRaise2 : GameTableTest
    {
        public override void Given(GameTableAggregate a)
        {
            a.CreateTable("123", "table", 100, 2);
            a.JoinTable("me1", 100);
            a.JoinTable("me2", 100);
            a.Raise("me2", 12);
            a.Raise("me1", 20);
        }

        public override void When(GameTableAggregate a)
        {
            a.Raise("me2", 12);
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new BidMade
            {
                Id = "123",
                BidType = BidTypeEnum.Raise,
                Bid = new BidInfo
                {
                    UserId = "me2",
                    Position = 2,
                    Bid = 26,
                    Odds = 12,
                    AllIn = false,
                    NewCashValue = 74
                }
            };
            yield return new NextPlayerTurned
            {
                Id = "123",
                Player = new PlayerInfo()
                {
                    UserId = "me1",
                    Position = 1
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