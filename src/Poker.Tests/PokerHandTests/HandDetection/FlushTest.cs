using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.ApplicationServices.Hands;
using Poker.Domain.Data;

namespace Poker.Tests.PokerHandTests.HandDetection
{
    public class FlushTest
    {
        [Test]
        public void flush_yes()
        {
            var set = new Flush();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Clubs, Rank.Seven)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void flush_yes2()
        {
            var set = new Flush();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Clubs, Rank.Six),
                new Card(Suit.Clubs, Rank.Five)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void flush_no()
        {
            var set = new Flush();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Spades, Rank.Seven)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsFalse(result);
        }

        [Test]
        public void flush_no2()
        {
            var set = new Flush();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Eight),
                new Card(Suit.Hearts, Rank.Eight),
                new Card(Suit.Spades, Rank.Seven)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsFalse(result);
        } 
    }
}