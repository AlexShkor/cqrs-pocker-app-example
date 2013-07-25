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
    public class PackCreationTest
    {
        [Test]
        public void create_new()
        {
            var pack = new Pack();
            Assert.AreEqual(52,pack.CardsCount());
        }
        
        [Test]
        public void create_with_all_cards()
        {
            var _cards = new List<Card>();
            foreach (var suit in Suit.GetAll())
            {
                foreach (var rank in Rank.GetAll())
                {
                    _cards.Add(new Card(suit, rank));
                }
            }
            var pack = new Pack(_cards);
            Assert.AreEqual(52,pack.CardsCount());
        }
    }
}
