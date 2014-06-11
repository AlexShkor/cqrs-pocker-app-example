using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.ApplicationServices.Hands;
using Poker.Domain.Data;

namespace Poker.Tests.PokerHandTests.HandDetection
{
    public class StraightFlushTest
    {
        [Test]
        public void straightflush_yes()
        {
            var set = new StraightFlush();
            var cards = new List<Card>
            {
                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Spades, Rank.Jack),
                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Nine),
                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Spades, Rank.Two)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void straightflush_no()
        {
            var set = new StraightFlush();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Ace),
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Hearts, Rank.King),
                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Spades, Rank.King)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsFalse(result);
        } 
    }
}