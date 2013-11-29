using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.ApplicationServices.Combinations;
using Poker.Domain.Data;

namespace Poker.Tests.PokerSetTests
{
    public class PockerSetsPresentingTests
    {
        [Test]
        public void one_pair_yes()
        {
            var set = new OnePair();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Hearts, Rank.Eight)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void one_pair_no()
        {
            var set = new OnePair();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Hearts, Rank.Five)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsFalse(result);
        }

        [Test]
        public void two_pair_yes()
        {
            var set = new TwoPairs();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Eight),
                new Card(Suit.Hearts, Rank.Five),
                new Card(Suit.Diamonds, Rank.Five)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void two_pair_no()
        {
            var set = new TwoPairs();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Eight),
                new Card(Suit.Hearts, Rank.Eight),
                new Card(Suit.Hearts, Rank.Ace)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsFalse(result);
        }

        [Test]
        public void two_pair_no2()
        {
            var set = new TwoPairs();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Eight),
                new Card(Suit.Hearts, Rank.Eight),
                new Card(Suit.Spades, Rank.Eight)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsFalse(result);
        }

        [Test]
        public void set_yes()
        {
            var set = new Set();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Eight),
                new Card(Suit.Hearts, Rank.Eight),
                new Card(Suit.Spades, Rank.Seven)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void set_no()
        {
            var set = new Set();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Eight),
                new Card(Suit.Hearts, Rank.Four),
                new Card(Suit.Spades, Rank.Seven)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsFalse(result);
        }

        [Test]
        public void straight_yes()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Clubs, Rank.Queen),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Diamonds, Rank.Seven),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Diamonds, Rank.Jack)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void straight_no()
        {
            var set = new Straight();
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
        public void fullhouse_yes()
        {
            var set = new FullHouse();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Ace),
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Spades, Rank.King)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void fullhouse_no()
        {
            var set = new FullHouse();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Ace),
                new Card(Suit.Spades, Rank.Seven),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Spades, Rank.King)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsFalse(result);
        }


        [Test]
        public void quads_yes()
        {
            var set = new Quads();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Ace),
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Spades, Rank.King)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void quads_no()
        {
            var set = new Quads();
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
            var set = new Quads();
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

        [Test]
        public void royalflush_yes()
        {
            var set = new StraightFlush();
            var cards = new List<Card>
            {
                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Spades, Rank.Jack),
                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Hearts, Rank.Nine)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void royalflush_yes2()
        {
            var set = new StraightFlush();
            var cards = new List<Card>
            {
                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Spades, Rank.Jack),
                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Spades, Rank.Nine)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void royalflush_no1()
        {
            var set = new StraightFlush();
            var cards = new List<Card>
            {
                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Spades, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Queen),
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Spades, Rank.Nine)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void royalflush_no2()
        {
            var set = new Quads();
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
