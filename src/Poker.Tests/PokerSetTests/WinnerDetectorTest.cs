using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using NUnit.Framework;
using Poker.Domain.ApplicationServices;
using Poker.Domain.Data;

namespace Poker.Tests.PokerSetTests
{
    [TestFixture]
    public class WinnerDetectorTest
    {
        [Test]
        public void HighCard_vs_HighCard()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                new Card(Suit.Clubs,Rank.King),
                new Card(Suit.Diamonds,Rank.Seven),
                new Card(Suit.Hearts,Rank.Queen),
                new Card(Suit.Clubs,Rank.Ten),
                new Card(Suit.Clubs,Rank.Two),
            });
            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Clubs,Rank.Three),
                new Card(Suit.Diamonds,Rank.Seven),
                new Card(Suit.Hearts,Rank.Queen),
                new Card(Suit.Clubs,Rank.Ten),
                new Card(Suit.Clubs,Rank.Two),
            });
            var winner = detector.GetWinners().Single();
            Assert.AreEqual("me1", winner.UserId);
        }

        [Test]
        public void Pair_vs_HighCard()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                new Card(Suit.Clubs,Rank.King),
                new Card(Suit.Diamonds,Rank.Ten),
                new Card(Suit.Hearts,Rank.Queen),
                new Card(Suit.Clubs,Rank.Ten),
                new Card(Suit.Clubs,Rank.Two),
            });
            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Clubs,Rank.Three),
                new Card(Suit.Diamonds,Rank.Seven),
                new Card(Suit.Hearts,Rank.Queen),
                new Card(Suit.Clubs,Rank.Ten),
                new Card(Suit.Clubs,Rank.Two),
            });
            var winner = detector.GetWinners().Single();
            Assert.AreEqual("me1", winner.UserId);
            Assert.AreEqual((int)PokerScores.OnePair, winner.PokerHand.Score);
        }

        [Test]
        public void TwoPairs_vs_Pair()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                new Card(Suit.Clubs,Rank.King),
                new Card(Suit.Diamonds,Rank.Ten),
                new Card(Suit.Hearts,Rank.Queen),
                new Card(Suit.Clubs,Rank.Ten),
                new Card(Suit.Clubs,Rank.Queen),
            });
            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Clubs,Rank.Three),
                new Card(Suit.Diamonds,Rank.Seven),
                new Card(Suit.Hearts,Rank.Ten),
                new Card(Suit.Clubs,Rank.Ten),
                new Card(Suit.Clubs,Rank.Two),
            });
            var winner = detector.GetWinners().Single();
            Assert.AreEqual("me1", winner.UserId);
            Assert.AreEqual((int)PokerScores.TwoPairs, winner.PokerHand.Score);
        }

        [Test]
        public void Set_vs_TwoPairs()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                new Card(Suit.Clubs,Rank.King),
                new Card(Suit.Diamonds,Rank.Ten),
                new Card(Suit.Hearts,Rank.Queen),
                new Card(Suit.Clubs,Rank.Ten),
                new Card(Suit.Clubs,Rank.Queen),
            });
            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Eight),
                new Card(Suit.Hearts, Rank.Eight),
                new Card(Suit.Spades, Rank.Seven)
            });
            var winner = detector.GetWinners().Single();
            Assert.AreEqual("me2", winner.UserId);
            Assert.AreEqual((int)PokerScores.Set, winner.PokerHand.Score);
        }

        [Test]
        public void Straight_vs_Set()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                  new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Clubs, Rank.Queen),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Diamonds, Rank.Seven),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Diamonds, Rank.Jack)
            });
            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Clubs, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Eight),
                new Card(Suit.Hearts, Rank.Eight),
                new Card(Suit.Spades, Rank.Seven)
            });
            var winner = detector.GetWinners().Single();
            Assert.AreEqual("me1", winner.UserId);
            Assert.AreEqual((int)PokerScores.Straight, winner.PokerHand.Score);
        }

        [Test]
        public void Straight_vs_Flush()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                  new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Clubs, Rank.Queen),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Diamonds, Rank.Seven),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Diamonds, Rank.Jack)
            });
            detector.AddPlayer("me2", new List<Card>()
            {
               new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Clubs, Rank.Seven)
            });
            var winner = detector.GetWinners().Single();
            Assert.AreEqual("me2", winner.UserId);
            Assert.AreEqual((int)PokerScores.Flush, winner.PokerHand.Score);
        }

        [Test]
        public void FullHouse_vs_Flush()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                 new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Ace),
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Spades, Rank.King)
            });
            detector.AddPlayer("me2", new List<Card>()
            {
               new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Clubs, Rank.Seven),
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Clubs, Rank.Seven)
            });
            var winner = detector.GetWinners().Single();
            Assert.AreEqual("me1", winner.UserId);
            Assert.AreEqual((int)PokerScores.FullHouse, winner.PokerHand.Score);
        }

        [Test]
        public void FullHouse_vs_Quads()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                 new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Ace),
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Clubs, Rank.Ten),
                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Spades, Rank.King)
            });
            detector.AddPlayer("me2", new List<Card>()
            {
               new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Ace),
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Spades, Rank.King)
            });
            var winner = detector.GetWinners().Single();
            Assert.AreEqual("me2", winner.UserId);
            Assert.AreEqual((int)PokerScores.Quads, winner.PokerHand.Score);
        }

        [Test]
        public void StraightFlush_vs_Quads()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                 new Card(Suit.Spades, Rank.King),
                new Card(Suit.Spades, Rank.Jack),
                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Nine),
                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Spades, Rank.Two)
            });
            detector.AddPlayer("me2", new List<Card>()
            {
               new Card(Suit.Clubs, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Ace),
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Spades, Rank.King)
            });
            var winner = detector.GetWinners().Single();
            Assert.AreEqual("me1", winner.UserId);
            Assert.AreEqual((int)PokerScores.StraightFlush, winner.PokerHand.Score);
        }

        [Test]
        public void StraightFlush_vs_RoyalFlush()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                 new Card(Suit.Spades, Rank.King),
                new Card(Suit.Spades, Rank.Jack),
                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Nine),
                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Spades, Rank.Two)
            });
            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Spades, Rank.Jack),
                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Spades, Rank.Ten),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Hearts, Rank.Nine)
            });
            var winner = detector.GetWinners().Single();
            Assert.AreEqual("me2", winner.UserId);
            Assert.AreEqual((int)PokerScores.RoyalFlush, winner.PokerHand.Score);
        }

        [Test]
        public void OnePair_vs_OnePair()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Hearts, Rank.King),

                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Nine),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Two)
            });

            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Clubs, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Jack),

                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Nine),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Two)
            });
            var winner = detector.GetWinners().Single();
            Assert.AreEqual("me1", winner.UserId);
            Assert.AreEqual((int)PokerScores.OnePair, winner.PokerHand.Score);
        }

        [Test]
        public void OnePair_vs_OnePair_Kicker()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Spades, Rank.Ace),

                new Card(Suit.Hearts, Rank.King),
                new Card(Suit.Spades, Rank.Nine),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Two)
            });
            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Spades, Rank.Queen),

                new Card(Suit.Hearts, Rank.King),
                new Card(Suit.Spades, Rank.Nine),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Two)
            });
            var winner = detector.GetWinners().Single();
            Assert.AreEqual("me1", winner.UserId);
            Assert.AreEqual((int)PokerScores.OnePair, winner.PokerHand.Score);
        }

        [Test]
        public void OnePair_vs_OnePair_SplitPot()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Hearts, Rank.King),


                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Nine),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Two)
            });
            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Diamonds, Rank.King),

                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Nine),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Two)
            });
            var winners = detector.GetWinners().ToList();
            Assert.AreEqual(2, winners.Count);
            Assert.AreEqual((int)PokerScores.OnePair, winners[0].PokerHand.Score);
            Assert.AreEqual((int)PokerScores.OnePair, winners[1].PokerHand.Score);
        }


        [Test]
        public void TwoPairs_vs_TwoPairs()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Hearts, Rank.King),

                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Two)
            });

            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Clubs, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Jack),

                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Two)
            });
            var winner = detector.GetWinners().Single();

            Assert.AreEqual("me1", winner.UserId);
            Assert.AreEqual((int)PokerScores.TwoPairs, winner.PokerHand.Score);
        }

        [Test]
        public void TwoPairs_vs_TwoPairs_when_hand_has_different_pairs()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                new Card(Suit.Spades, Rank.Four),
                new Card(Suit.Hearts, Rank.Four),

                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Spades, Rank.Six),
                new Card(Suit.Diamonds, Rank.Jack),
                new Card(Suit.Clubs, Rank.Jack)
            });

            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Clubs, Rank.Three),
                new Card(Suit.Diamonds, Rank.Six),

                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Spades, Rank.Six),
                new Card(Suit.Diamonds, Rank.Jack),
                new Card(Suit.Clubs, Rank.Jack)
            });

            var winner = detector.GetWinners().Single();

            Assert.AreEqual("me2", winner.UserId);
            Assert.AreEqual((int)PokerScores.TwoPairs, winner.PokerHand.Score);
        }


        [Test]
        public void TwoPairs_vs_TwoPairs_Kicker()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Hearts, Rank.Three),

                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Spades, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Two)
            });

            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Diamonds, Rank.Jack),

                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Spades, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Two)
            });
            var winner = detector.GetWinners().Single();

            Assert.AreEqual("me2", winner.UserId);
            Assert.AreEqual((int)PokerScores.TwoPairs, winner.PokerHand.Score);
        }


        [Test]
        public void TwoPairs_vs_TwoPairs_SplitPot()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Hearts, Rank.Queen),

                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Two)
            });
            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Clubs, Rank.King),
                new Card(Suit.Diamonds, Rank.Queen),

                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Spades, Rank.Queen),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Two)
            });
            var winners = detector.GetWinners().ToList();

            Assert.AreEqual(2, winners.Count);
            Assert.AreEqual((int)PokerScores.TwoPairs, winners[0].PokerHand.Score);
            Assert.AreEqual((int)PokerScores.TwoPairs, winners[1].PokerHand.Score);
        }

        [Test]
        public void Set_vs_Set()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Hearts, Rank.Ace),

                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Spades, Rank.Jack),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Two)
            });

            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Clubs, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Jack),

                new Card(Suit.Spades, Rank.Ace),
                new Card(Suit.Spades, Rank.Jack),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Clubs, Rank.Two)
            });
            var winner = detector.GetWinners().Single();

            Assert.AreEqual("me1", winner.UserId);
            Assert.AreEqual((int)PokerScores.Set, winner.PokerHand.Score);
        }


        [Test]
        public void Set_vs_Set_when_hand_has_different_sets()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Hearts, Rank.Three),

                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Spades, Rank.Jack),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Clubs, Rank.Five)
            });

            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Clubs, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Jack),

                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Spades, Rank.Jack),
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Clubs, Rank.Five)
            });
            var winner = detector.GetWinners().Single();

            Assert.AreEqual("me2", winner.UserId);
            Assert.AreEqual((int)PokerScores.Set, winner.PokerHand.Score);
        }

        [Test]
        public void Set_vs_Set_Kicker()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Hearts, Rank.King),

                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Diamonds, Rank.King),
                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Diamonds, Rank.Ten),
                new Card(Suit.Clubs, Rank.Two)
            });

            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Clubs, Rank.Six),
                new Card(Suit.Clubs, Rank.King),

                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Diamonds, Rank.King),
                new Card(Suit.Spades, Rank.Three),
                new Card(Suit.Diamonds, Rank.Ten),
                new Card(Suit.Clubs, Rank.Two)
            });
            var winner = detector.GetWinners().Single();

            Assert.AreEqual("me2", winner.UserId);
            Assert.AreEqual((int)PokerScores.Set, winner.PokerHand.Score);
        }

        [Test]
        public void Set_vs_Set_SplitPot()
        {
            var detector = new WinnerDetector();
            detector.AddPlayer("me1", new List<Card>()
            {
                new Card(Suit.Spades, Rank.Five),
                new Card(Suit.Hearts, Rank.King),

                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Diamonds, Rank.King),
                new Card(Suit.Spades, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Ten),
                new Card(Suit.Clubs, Rank.Two)
            });

            detector.AddPlayer("me2", new List<Card>()
            {
                new Card(Suit.Clubs, Rank.Six),
                new Card(Suit.Clubs, Rank.King),

                new Card(Suit.Spades, Rank.King),
                new Card(Suit.Diamonds, Rank.King),
                new Card(Suit.Spades, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Ten),
                new Card(Suit.Clubs, Rank.Two)
            });


            var winners = detector.GetWinners().ToList();

            Assert.AreEqual(2, winners.Count);
            Assert.AreEqual((int)PokerScores.Set, winners[0].PokerHand.Score);
            Assert.AreEqual((int)PokerScores.Set, winners[1].PokerHand.Score);
        }
    }
}