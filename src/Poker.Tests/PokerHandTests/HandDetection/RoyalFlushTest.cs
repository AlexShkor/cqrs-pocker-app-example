using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.ApplicationServices.Hands;
using Poker.Domain.Data;

namespace Poker.Tests.PokerHandTests.HandDetection
{
    public class RoyalFlushTest
    {
        [Test]
        public void royalflush_yes()
        {
            var set = new RoyalFlush();
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
            var set = new RoyalFlush();
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
        public void royalflush_yes3()
        {
            var set = new RoyalFlush();
            var cards = new List<Card>()
            {
                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Diamonds, Rank.Ten),

                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Hearts, Rank.King),
                new Card(Suit.Hearts, Rank.Queen),
                new Card(Suit.Hearts, Rank.Jack),
                new Card(Suit.Hearts, Rank.Ten)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void royalflush_no1()
        {
            var set = new RoyalFlush();
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
            Assert.IsFalse(result);
        }

        [Test]
        public void royalflush_no2()
        {
            var set = new RoyalFlush();
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
        public void royalflush_no3()
        {
            var set = new RoyalFlush();
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
            Assert.IsFalse(result);
        }

        [Test]
        public void royalflush_no4()
        {
            var set = new RoyalFlush();
            var cards = new List<Card>
            {
                new Card(Suit.Diamonds, Rank.Ace),
                new Card(Suit.Clubs, Rank.Queen),
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Clubs, Rank.Nine),
                new Card(Suit.Clubs, Rank.Jack)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsFalse(result);
        } 
    }
}