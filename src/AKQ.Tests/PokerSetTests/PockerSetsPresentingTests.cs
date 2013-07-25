using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKQ.Domain;
using AKQ.Domain.PokerSets;
using NUnit.Framework;

namespace AKQ.Tests.PokerSetTests
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
