using NUnit.Framework;
using Poker.Domain.ApplicationServices;
using Poker.Domain.ApplicationServices.Hands;

namespace Poker.Tests.PokerHandTests
{
    [TestFixture]
    public class HandScoresTest
    {
        [Test]
        public void test_scores()
        {
            var highCard = new HighCard();
            Assert.AreEqual(0, highCard.Score);
            var onePair = new OnePair();
            Assert.AreEqual(1, onePair.Score);
            var twoPairs = new TwoPairs();
            Assert.AreEqual(2, twoPairs.Score);
            var set = new Set();
            Assert.AreEqual(3, set.Score);
            var straight = new Straight();
            Assert.AreEqual(4, straight.Score);
            var flush = new Flush();
            Assert.AreEqual(5, flush.Score);
            var fullHouse = new FullHouse();
            Assert.AreEqual(6, fullHouse.Score);
            var quads = new Quads();
            Assert.AreEqual(7, quads.Score);
            var straightFlush = new StraightFlush();
            Assert.AreEqual(8, straightFlush.Score);
            var royalFlush = new RoyalFlush();
            Assert.AreEqual(9, royalFlush.Score);
        }
    }
}