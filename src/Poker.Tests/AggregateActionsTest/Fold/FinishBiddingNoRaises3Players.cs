﻿using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Platform.Domain.Interfaces;

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
            yield return new BidMade
            {
                Id = "123",
                Bid = new BidInfo
                {
                    UserId = "me1",
                    Bid = 8,
                    BidType = BidTypeEnum.Fold,
                    BiddingStage = 0,
                    NewCashValue = 92,
                    Position = 1,
                    Odds = 4
                }
            };
            yield return new GameFinished
            {
                Id = "123",
                Winners = new List<WinnerInfo>
                {
                    new WinnerInfo
                    {
                        UserId = "me2",
                        Position = 2,
                        Amount = 10,
                        HandScore = 0
                    }
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