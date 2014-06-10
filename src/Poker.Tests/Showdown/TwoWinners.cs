using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Poker.Domain.ApplicationServices;
using Poker.Domain.ApplicationServices.Hands;
using Poker.Domain.Data;

namespace Poker.Tests.Showdown
{
    [TestFixture]
    public class TwoWinners
    {
        [Test]
        public void two()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Hearts, Rank.Queen),

                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Two)
            });
            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Diamonds, Rank.Queen),

                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Two)
            });
            var winners = detector.GetWinners(100).ToList();
            Assert.AreEqual(2, winners.Count);
            Assert.AreEqual(winners[0].Prize, 50);
            Assert.AreEqual(winners[0].PokerHand.GetType(), typeof(TwoPairs));
            Assert.AreEqual(winners[1].Prize, 50);
            Assert.AreEqual(winners[1].PokerHand.GetType(), typeof(TwoPairs));
            var ids = winners.Select(x => x.UserId).ToList();
            Assert.Contains("me1", ids);
            Assert.Contains("me2", ids);
        }
    }
}