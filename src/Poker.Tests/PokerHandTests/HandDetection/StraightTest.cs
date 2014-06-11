using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Poker.Domain.ApplicationServices.Hands;
using Poker.Domain.Data;
using Poker.Tests.PokerSetTests;

namespace Poker.Tests.PokerHandTests.HandDetection
{
    public class StraightTest
    {
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
            var highCard = set.HandCards.OrderByDescending(x => x.Rank).First();

            Assert.IsTrue(result);
            Assert.AreEqual(Rank.Ace.Score, highCard.Rank.Score);
        }


        [Test]
        public void straight_yes_2()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Queen),
                new Card(Suit.Diamonds, Rank.Jack),

                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Clubs, Rank.Two),
                new Card(Suit.Diamonds, Rank.Three),
                new Card(Suit.Clubs, Rank.Four),
                new Card(Suit.Diamonds, Rank.Five),
            };


            set.SetCards(cards);
            var result = set.IsPresent();
            var highCard = set.HandCards.OrderByDescending(x => x.Rank).First(); /* ace as one */

            Assert.IsTrue(result);
            Assert.AreEqual(Rank.Ace.Score, highCard.Rank.Score);
        }

        [Test]
        public void straight_yes_3()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Diamonds, Rank.Two),

                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Diamonds, Rank.Queen),
                new Card(Suit.Clubs, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Ten)
            };

            set.SetCards(cards);
            var result = set.IsPresent();
            var highCard = set.HandCards.OrderByDescending(x => x.Rank).First();

            Assert.IsTrue(result);
            Assert.AreEqual(Rank.Ace.Score, highCard.Rank.Score);
        }

        [Test]
        public void straight_yes_4()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Diamonds, Rank.Two),

                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Clubs, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Eight),
                new Card(Suit.Clubs, Rank.Nine),
                new Card(Suit.Diamonds, Rank.Seven)
            };


            set.SetCards(cards);
            var result = set.IsPresent();
            var highCard = set.HandCards.OrderByDescending(x => x.Rank).First();

            Assert.IsTrue(result);
            Assert.AreEqual(Rank.Jack.Score, highCard.Rank.Score);
        }


        [Test]
        public void straight_yes_5()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Eight),

                new Card(Suit.Spades, Rank.Two),
                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Diamonds, Rank.Four),
                new Card(Suit.Clubs, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six)
            };

            set.SetCards(cards);
            var result = set.IsPresent();
            var highCard = set.HandCards.OrderByDescending(x => x.Rank).First();

            Assert.IsTrue(result);
            Assert.AreEqual(Rank.Eight.Score, highCard.Rank.Score);
        }

        [Test]
        public void straight_yes_6()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Nine),

                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Clubs, Rank.Four),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Clubs, Rank.Six),
                new Card(Suit.Diamonds, Rank.Seven)
            };

            set.SetCards(cards);
            var result = set.IsPresent();
            var highCard = set.HandCards.OrderByDescending(x => x.Rank).First();

            Assert.IsTrue(result);
            Assert.AreEqual(Rank.Nine.Score, highCard.Rank.Score);
        }

        [Test]
        public void straight_yes_7()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Ace),

                new Card(Suit.Spades, Rank.Four),
                new Card(Suit.Clubs, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Eight)
                
            };

            set.SetCards(cards);
            var result = set.IsPresent();
            var highCard = set.HandCards.OrderByDescending(x => x.Rank).First();

            Assert.IsTrue(result);
            Assert.AreEqual(Rank.Eight.Score, highCard.Rank.Score);
        }

        [Test]
        public void straight_yes_8()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Ace),

                new Card(Suit.Spades, Rank.Two),
                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Diamonds, Rank.Four),
                new Card(Suit.Clubs, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six)
               
            };

            set.SetCards(cards);
            var result = set.IsPresent();
            var highCard = set.HandCards.OrderByDescending(x => x.Rank).First();

            Assert.IsTrue(result);
            Assert.AreEqual(Rank.Seven.Score, highCard.Rank.Score);
        }

        [Test]
        public void straight_yes_9()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Ace),

                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Clubs, Rank.Queen),
                new Card(Suit.Diamonds, Rank.Jack),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Diamonds, Rank.Six)
             
            };

            set.SetCards(cards);
            var result = set.IsPresent();
            var highCard = set.HandCards.OrderByDescending(x => x.Rank).First();

            Assert.IsTrue(result);
            Assert.AreEqual(Rank.Ace.Score, highCard.Rank.Score);
        }

        [Test]
        public void straight_yes_10()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Ace),

                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Clubs, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Ten),
                new Card(Suit.Clubs, Rank.Nine),
                new Card(Suit.Diamonds, Rank.Eight)
            };

            set.SetCards(cards);
            var result = set.IsPresent();
            var highCard = set.HandCards.OrderByDescending(x => x.Rank).First();

            Assert.IsTrue(result);
            Assert.AreEqual(Rank.Jack.Score, highCard.Rank.Score);
        }

        [Test]
        public void straight_yes_11()
        {
            var set = new Straight();
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
            var highCard = set.HandCards.OrderByDescending(x => x.Rank).First();

            Assert.IsTrue(result);
            Assert.AreEqual(Rank.Ace.Score, highCard.Rank.Score);
        }


        [Test]
        public void straight_yes_12()
        {
            var set = new Straight();
            var cards = new List<Card>()
            {
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Two),

                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Diamonds, Rank.Four),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Hearts, Rank.Six),
                new Card(Suit.Spades, Rank.Seven)
            };

            set.SetCards(cards);
            var result = set.IsPresent();
            var highCard = set.HandCards.OrderByDescending(x => x.Rank).First();

            Assert.IsTrue(result);
            Assert.AreEqual(Rank.Seven.Score, highCard.Rank.Score);
        }


        [Test]
        public void straight_no()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Diamonds, Rank.Two),
                new Card(Suit.Hearts, Rank.Three),

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
        public void straight_no_2()
        {
            var set = new Straight();
            var cards = new List<Card>
            {

                new Card(Suit.Clubs, Rank.Two),
                new Card(Suit.Diamonds, Rank.Jack),

                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Clubs, Rank.Four),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Clubs, Rank.Six),
                new Card(Suit.Diamonds, Rank.Seven)
              
            };

            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsFalse(result);
        }

        [Test]
        public void straight_no_3()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Diamonds, Rank.Jack),

                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Clubs, Rank.Four),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Clubs, Rank.Six),
                new Card(Suit.Diamonds, Rank.Seven)
            };

            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsFalse(result);
        }

        [Test]
        public void streight_handCards()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Two),

                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Hearts, Rank.Five),
                new Card(Suit.Spades, Rank.Four),
                new Card(Suit.Spades, Rank.Nine),
                new Card(Suit.Clubs, Rank.Queen)
            };

            set.SetCards(cards);
            var result = set.IsPresent();
            var handCards = set.HandCards.OrderBy(x => x.Rank.Score).ToList();

            Assert.True(result);
            handCards[0].AssertRank(Rank.Two);
            handCards[1].AssertRank(Rank.Three);
            handCards[2].AssertRank(Rank.Four);
            handCards[3].AssertRank(Rank.Five);
            handCards[4].AssertRank(Rank.Ace);
        }

        [Test]
        public void streight_handCards_2()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Two),

                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Hearts, Rank.Five),
                new Card(Suit.Spades, Rank.Four),
                new Card(Suit.Spades, Rank.Nine),
                new Card(Suit.Clubs, Rank.Six)
            };

            set.SetCards(cards);
            var result = set.IsPresent();
            var handCards = set.HandCards.OrderBy(x => x.Rank.Score).ToList();

            Assert.True(result);
            handCards[0].AssertRank(Rank.Two);
            handCards[1].AssertRank(Rank.Three);
            handCards[2].AssertRank(Rank.Four);
            handCards[3].AssertRank(Rank.Five);
            handCards[4].AssertRank(Rank.Six);
        }

        [Test]
        public void streight_handCards_3()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Six),
                new Card(Suit.Diamonds, Rank.Two),

                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Hearts, Rank.Five),
                new Card(Suit.Spades, Rank.Four),
                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Clubs, Rank.Seven)
            };

            set.SetCards(cards);
            var result = set.IsPresent();
            var handCards = set.HandCards.OrderBy(x => x.Rank.Score).ToList();

            Assert.True(result);
            handCards[0].AssertRank(Rank.Three);
            handCards[1].AssertRank(Rank.Four);
            handCards[2].AssertRank(Rank.Five);
            handCards[3].AssertRank(Rank.Six);
            handCards[4].AssertRank(Rank.Seven);
        }
    }
}