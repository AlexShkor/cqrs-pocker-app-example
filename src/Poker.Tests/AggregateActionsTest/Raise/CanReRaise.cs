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
    public class CanReRaise : GameTableTest
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
            a.Raise("me2", 20);
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new BidMade
            {
                Id = "123",
                Bid = new BidInfo
                {
                    UserId = "me2",
                    Position = 2,
                    Bet = 40,
                    Bid = 40,
                    Amount = 20,
                    BidType = BidTypeEnum.Raise,
                    NewCashValue = 60
                }
            };

            yield return new NextPlayerTurned
            {
                Id = "123",
                Player = new PlayerInfo()
                {
                    UserId = "me1",
                    Position = 1
                },
                MinBet = 50,
                MaxRaisedValue = 10
            };
        }

        public override void ValidateState(GameTableAggregate a)
        {
            Assert.AreEqual(40, a.State.CurrentBidding.CurrentStage.Bids[2].Bid);
            Assert.AreEqual(20, a.State.CurrentBidding.CurrentStage.Bids[2].Amount);
            Assert.AreEqual(2, a.State.CurrentBidding.CurrentStage.Bids[2].Position);
        }

        [Test]
        public override void Test()
        {
            ValidateEvents("GameId");
            ValidateState();
        }
    }
}
