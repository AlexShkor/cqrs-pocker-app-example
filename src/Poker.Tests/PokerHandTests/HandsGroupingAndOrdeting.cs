using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Poker.Domain.ApplicationServices;
using Poker.Domain.ApplicationServices.Hands;
using Poker.Domain.Data;

namespace Poker.Tests.PokerSetTests
{
    [TestFixture]
    public class HandsGroupingAndOrdeting
    {
        [Test]
        public void test()
        {
            var first = new OnePair();
            first.SetCards(new List<Card>
            {
                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Eight),
                new Card(Suit.Hearts, Rank.Two),
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Seven),
            });
            Assert.IsTrue(first.IsPresent());
            var second = new OnePair();
            second.SetCards(new List<Card>
            {
                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Eight),
                new Card(Suit.Hearts, Rank.Two),
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Seven),
            });
            Assert.IsTrue(second.IsPresent());
            var third = new OnePair();
            third.SetCards(new List<Card>
            {
                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Eight),
                new Card(Suit.Hearts, Rank.Two),
                new Card(Suit.Spades, Rank.Four),
                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Seven),
            });
            Assert.IsTrue(third.IsPresent());
            var hands = new List<IPokerHand>
            {
                first,
                second,
                third
            };
            Assert.AreEqual(0, first.CompareTo(second));
            var sortedList = new WinnerDetector().GetOrdered(hands);
            Assert.AreEqual(2, sortedList[1].Count());
            Assert.AreEqual(1, sortedList[2].Count());
            Assert.AreSame(third, sortedList[2].First());
        }
    }
}