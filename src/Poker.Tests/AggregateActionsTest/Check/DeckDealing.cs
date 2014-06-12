using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Tests.AggregateActionsTest.Check
{
    [TestFixture]
    public class DeckDealing : GameTableTest
    {
        public override void Given(GameTableAggregate a)
        {
            a.CreateTable("123", "table", 100, 2);
            a.JoinTable("me1", 100);
            a.JoinTable("me2", 100);
            a.Call("me2");
        }

        public override void When(GameTableAggregate a)
        {
            a.Check("me1");
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new BidMade
            {
                Id = "123",
                Bid = new BidInfo
                {
                    UserId = "me2",
                    Bid = 4,
                    BidType = BidTypeEnum.Check,
                    BiddingStage = 0,
                    NewCashValue = 96,
                    Position = 2,
                    Odds = 0
                }
            };
            yield return new DeckDealed
            {
                Id = "123"
            };
            yield return new NextPlayerTurned
            {
                Id = "123",
                Player = new PlayerInfo(2,"me2")
            };
        }

        [Test]
        public override void Test()
        {
            Assert.AreEqual(3,GetChanges<DeckDealed>().First().Cards.Count);
            ValidateEvents("GameId", "Cards");
        }
    }
}