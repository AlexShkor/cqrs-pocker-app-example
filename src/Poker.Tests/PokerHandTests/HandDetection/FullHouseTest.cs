using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.ApplicationServices.Hands;
using Poker.Domain.Data;

namespace Poker.Tests.PokerHandTests.HandDetection
{
    public class FullHouseTest
    {
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
 
    }
}