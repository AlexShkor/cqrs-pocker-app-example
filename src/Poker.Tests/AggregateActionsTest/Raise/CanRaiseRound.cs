using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Tests.AggregateActionsTest.Raise
{
    public class CanRaiseRound : GameTableTest
    {
        public override void Given(GameTableAggregate a)
        {
            a.CreateTable("123", "table", 100, 5);
            a.JoinTable("me1", 100);
            a.JoinTable("me2", 100);
            a.Raise("me2", 10);
            a.Raise("me1", 10);
        }

        public override void When(GameTableAggregate a)
        {
            a.Raise("me2", 10);
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
                    Bid = 25,
                    Odds = 10,
                    AllIn = false,
                    NewCashValue = 75
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

        public override void ValidateState(GameTableAggregate a)
        {
            Assert.AreEqual(5, a.State.CurrentBidding.CurrentStage.Bids[0].Odds);
            Assert.AreEqual(2, a.State.CurrentBidding.CurrentStage.Bids[0].Position);
        }

        [Test]
        public override void Test()
        {
            ValidateEvents("GameId");
            ValidateState();
        }
    }
}
