using AKQ.Domain;
using NUnit.Framework;

namespace AKQ.Tests
{
    [TestFixture]
    public class TrickTest
    {

        [Test]
        public void SameSuit()
        {
            var trick = new Trick(Suit.NoTrumps)
                .West(Suit.Hearts, Rank.Ace)
                .East(Suit.Hearts, Rank.King)
                .North(Suit.Hearts, Rank.Queen)
                .South(Suit.Hearts, Rank.Jack);
            Assert.AreEqual(PlayerPosition.West, trick.Winner);
        }

        [Test]
        public void NotLedSuit()
        {
            var trick = new Trick(Suit.NoTrumps)
                .West(Suit.Hearts, Rank.Two)
                .East(Suit.Spades, Rank.King)
                .North(Suit.Clubs, Rank.Queen)
                .South(Suit.Diamonds, Rank.Jack);
            Assert.AreEqual(PlayerPosition.West, trick.Winner);
        }
        
        [Test]
        public void NotLedSuit2()
        {
            var trick = new Trick(Suit.Clubs)
                .West(Suit.Hearts, Rank.King)
                .North(Suit.Clubs, Rank.Three)
                .East(Suit.Hearts, Rank.Two)
                .South(Suit.Hearts, Rank.Six);
            Assert.AreEqual(PlayerPosition.North, trick.Winner);
        }

        [Test]
        public void WithTrumpSuit()
        {
            var trick = new Trick(Suit.Spades)
                .West(Suit.Hearts, Rank.Ace)
                .East(Suit.Spades, Rank.King)
                .North(Suit.Clubs, Rank.Queen)
                .South(Suit.Diamonds, Rank.Ace);
            Assert.AreEqual(PlayerPosition.East, trick.Winner);
        }

        [Test]
        public void WithTrumpSuit2()
        {
            var trick = new Trick(Suit.Spades)
                .West(Suit.Hearts, Rank.Ace)
                .East(Suit.Hearts, Rank.King)
                .North(Suit.Spades, Rank.Two)
                .South(Suit.Hearts, Rank.Jack);
            Assert.AreEqual(PlayerPosition.North, trick.Winner);
        }

        [Test]
        public void WithTrumpSuit3()
        {
            var trick = new Trick(Suit.Clubs)
                .South(Suit.Diamonds, Rank.Ace)
                .West(Suit.Diamonds, Rank.Three)
                .North(Suit.Diamonds, Rank.Eight)
                .East(Suit.Clubs, Rank.Three);
            Assert.AreEqual(PlayerPosition.East, trick.Winner);
        }

        [Test]
        public void WithNoTrumpSuit()
        {
            var trick = new Trick(Suit.NoTrumps)
                .West(Suit.Spades, Rank.Jack)
                .North(Suit.Spades, Rank.Two)
                .East(Suit.Spades, Rank.Seven)
                .South(Suit.Clubs, Rank.Three);
            Assert.AreEqual(PlayerPosition.West, trick.Winner);
        }
    }
}
