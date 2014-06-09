using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.ApplicationServices.Hands;
using Poker.Domain.Data;

namespace Poker.Tests.PokerHandTests.HandDetection
{

    public class OnePairTest
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
        public void one_pair_no2()
        {
            var set = new OnePair();
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