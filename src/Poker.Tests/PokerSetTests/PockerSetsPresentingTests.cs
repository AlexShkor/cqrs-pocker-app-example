using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.ApplicationServices;
using Poker.Domain.Data;

namespace Poker.Tests.PokerSetTests
{
    public class PockerSetsPresentingTests
    {
        [Test]
        public void one_pair()
        {
            var set = new OnePairSet();
            var cards = new List<Card> {new Card(Suit.Clubs, Rank.Eight), new Card(Suit.Hearts, Rank.Eight)};
            var result = set.IsPresent(cards);
            Assert.IsTrue(result);
        }
    }
}
