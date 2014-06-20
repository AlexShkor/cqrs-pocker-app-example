using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Tests.AggregateActionsTest.Fold
{
    public class FinishesBidding2Players : GameTableTest
    {
        public override void Given(GameTableAggregate a)
        {
            a.CreateTable("123", "table", 100, 2);
            a.JoinTable("me1", 100);
            a.JoinTable("me2", 100);
        }

        public override void When(GameTableAggregate a)
        {
            a.Fold("me2");
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new BidMade
            {
                Id = "123",
                Bid = new BidInfo
                {
                    UserId = "me2",
                    Bid = 2,
                    BidType = BidTypeEnum.Fold,
                    BiddingStage = 0,
                    NewCashValue = 98,
                    Position = 2,
                    Odds = 0
                }
            };
            yield return new GameFinished
            {
                Id = "123",
                Winners = new List<WinnerInfo> { new WinnerInfo
                {
                    UserId = "me1",
                    Position = 1,
                    Amount = 6,
                    HandScore = 1
                 }
              }
            };

            yield return new GameCreated()
            {
                Id = "123"
            };

            yield return new CardsDealed()
            {
                Id = "123"
            };

            yield return new DealerAssigned()
            {
                Id = "123"
            };

            yield return new BidMade()
            {
                Id = "123"
            };

            yield return new BidMade()
            {
                Id = "123"
            };

            yield return new NextPlayerTurned()
            {
                Id = "123"
            };
        }

        [Test]
        public override void Test()
        {
            ValidateEvents("GameId", "Players", "Cards", "Dealer", "SmallBlind", "BigBlind", "Bid", "Player");
        }
    }
}