using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Poker.Domain.ApplicationServices;
using Poker.Domain.Data;

namespace Poker.Tests.Showdown
{
    [TestFixture]
    public class OneWinner
    {
        [Test]
        public void test()
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
            var winners = detector.GetWinners(100).ToList();
        }
    }
}