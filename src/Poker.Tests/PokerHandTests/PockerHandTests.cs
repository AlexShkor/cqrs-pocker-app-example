using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Poker.Domain.ApplicationServices.Hands;
using Poker.Domain.Data;

namespace Poker.Tests.PokerSetTests
{
    public class PockerSetsTests
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
        public void two_pair_no3()
        {
            var set = new TwoPairs();
            var cards = new List<Card>
            {
                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Eight),
                new Card(Suit.Hearts, Rank.Eight),
                new Card(Suit.Spades, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Seven)
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
        public void straight_yes_2()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Clubs, Rank.Two),
                new Card(Suit.Diamonds, Rank.Three),
                new Card(Suit.Clubs, Rank.Four),
                new Card(Suit.Diamonds, Rank.Five),

                new Card(Suit.Clubs, Rank.Queen),
                new Card(Suit.Diamonds, Rank.Jack)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void straight_yes_3()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Diamonds, Rank.Queen),
                new Card(Suit.Clubs, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Ten),

                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Diamonds, Rank.Two)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void straight_yes_4()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Clubs, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Eight),
                new Card(Suit.Clubs, Rank.Nine),
                new Card(Suit.Diamonds, Rank.Seven),

                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Diamonds, Rank.Two)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }


        [Test]
        public void straight_yes_5()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Spades, Rank.Two),
                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Diamonds, Rank.Four),
                new Card(Suit.Clubs, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),

                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Eight)
            };
            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void straight_yes_6()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Clubs, Rank.Four),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Clubs, Rank.Six),
                new Card(Suit.Diamonds, Rank.Seven),

                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Nine)
            };

            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }


        [Test]
        public void straight_yes_7()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Spades, Rank.Four),
                new Card(Suit.Clubs, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Eight),

                new Card(Suit.Clubs, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Ace)
            };

            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void straight_yes_8()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Spades, Rank.Two),
                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Diamonds, Rank.Four),
                new Card(Suit.Clubs, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),

                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Ace)
            };

            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void straight_yes_9()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Clubs, Rank.Queen),
                new Card(Suit.Diamonds, Rank.Jack),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Diamonds, Rank.Six),

                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Ace)
            };

            set.SetCards(cards);
            var result = set.IsPresent();
            Assert.IsTrue(result);
        }

        [Test]
        public void straight_yes_10()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Clubs, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Ten),
                new Card(Suit.Clubs, Rank.Nine),
                new Card(Suit.Diamonds, Rank.Eight),

                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Ace)
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
        public void straight_no_2()
        {
            var set = new Straight();
            var cards = new List<Card>
            {
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Clubs, Rank.Four),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Clubs, Rank.Six),
                new Card(Suit.Diamonds, Rank.Seven),

                new Card(Suit.Clubs, Rank.Two),
                new Card(Suit.Diamonds, Rank.Jack)
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
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Clubs, Rank.Four),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Clubs, Rank.Six),
                new Card(Suit.Diamonds, Rank.Seven),

                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Diamonds, Rank.Jack)
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
            Assert.True(result);
            var handCards = set.HandCards.OrderBy(x => x.Rank.Score).ToList();
            handCards[0].AssertRank(Rank.Two);
            handCards[1].AssertRank(Rank.Three);
            handCards[2].AssertRank(Rank.Four);
            handCards[3].AssertRank(Rank.Five);
            handCards[4].AssertRank(Rank.Ace);
        }

        [Test]
        public void streight_handCards2()
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
            Assert.True(result);
            var handCards = set.HandCards.OrderBy(x => x.Rank.Score).ToList();
            handCards[0].AssertRank(Rank.Three);
            handCards[1].AssertRank(Rank.Four);
            handCards[2].AssertRank(Rank.Five);
            handCards[3].AssertRank(Rank.Six);
            handCards[4].AssertRank(Rank.Seven);
        }

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

    public static class CardExt
    {
        public static void AssertRank(this Card card, Rank rank)
        {
            Assert.AreEqual(rank, card.Rank);
        }
    }
}