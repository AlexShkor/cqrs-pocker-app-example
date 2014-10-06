using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Domain.Data;

namespace Poker.Tests.AggregateStateTests
{

    public class PlayerFoldBidTest : GameSetUp
    {
        [Test]
        public void PlayerState()
        {
            _state.Invoke(new BidMade
            {
                Id = "game1",
                Bid = new BidInfo
                {
                    UserId = "me2",
                    Bid = 5,
                    BidType = BidTypeEnum.Fold,
                    BiddingStage = 0,
                    NewCashValue = 995,
                    Position = 2,
                    Amount = 0
                }
            });

            Assert.IsTrue(_state.Players[2].Fold);
            Assert.AreEqual(5, _state.Players[2].Bid);
        }
    }
}
