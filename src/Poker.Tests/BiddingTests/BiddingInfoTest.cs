using NUnit.Framework;
using Poker.Domain.Aggregates.Game;

namespace Poker.Tests.BiddingTests
{
    [TestFixture]
    public class BiddingInfoTest
    {
        [Test]
        public void init_stage_when_bidding_inits()
        {
            var bidding = new BiddingInfo(1);
            Assert.NotNull(bidding.CurrentStage);
        }

        [Test]
        public void calculate_bank_for_one_stage_and_one_bid_per_player()
        {
            var bidding = new BiddingInfo(2);
            SetBank60ForTwoPlayers(bidding);
            Assert.AreEqual(60, bidding.GetBank());
        }

        [Test]
        public void calculate_bank_for_two_stages_and_one_bid_per_player()
        {
            var bidding = new BiddingInfo(2);
            SetBank60ForTwoPlayers(bidding);
            bidding.NextStage();
            SetBank60ForTwoPlayers(bidding);
            Assert.AreEqual(60, bidding.GetBank());
        }

        public void SetBank60ForTwoPlayers(BiddingInfo bidding)
        {
            bidding.AddTestBid(2, 5);
            bidding.AddTestBid(1, 10);
            bidding.AddTestBid(2, 20);
            bidding.AddTestBid(1, 30);
            bidding.AddTestBid(2, 30);
        }
    }

    internal static class BiddingInfoExtenssionForTest
    {
        public static void AddTestBid(this BiddingInfo biddingInfo, int position, long bid)
        {
            biddingInfo.AddBid(new BidInfo
            {
                Bid = bid,
                BiddingStage = biddingInfo.Stage,
                Position = position,
                UserId = "me" + position
            });
        }
    }
}