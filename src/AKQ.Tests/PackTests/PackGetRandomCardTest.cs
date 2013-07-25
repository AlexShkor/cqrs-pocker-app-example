using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKQ.Domain;
using NUnit.Framework;
using AKQ.Domain.Bridge;

namespace AKQ.Tests.PackTests
{
    public class PackGetRandomCardTest
    {
        [Test]
        public void can_get_one()
        {
            var pack = new Pack();
            var card = pack.TakeRandom();
            Assert.Pass("Card given:" + card.ToString());
        }

        [Test]
        public void throws_exception_if_empty()
        {
            var pack = new Pack();
            for (int i = 0; i < 52; i++)
            {
                pack.TakeRandom();
            }
            Assert.IsTrue(pack.IsEmpty);
            Assert.Throws<InvalidOperationException>(() => pack.TakeRandom());
        }

        [Test]
        public void gives_only_different_cards()
        {
            var pack = new Pack();
            var cards = new List<Card>();
            for (int i = 0; i < 52; i++)
            {
                cards.Add(pack.TakeRandom());
            }
            Assert.IsTrue(pack.IsEmpty);
            for (int i = 0; i < 51; i++)
            {
                for (int j = i + 1; j < 52; j++)
                {
                    Assert.AreNotSame(cards[i],cards[j]);
                }
            }
        }
    }
}
