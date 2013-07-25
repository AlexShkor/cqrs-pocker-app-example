using System.Threading.Tasks;
using AKQ.Domain;
using NUnit.Framework;

namespace AKQ.Tests
{
    //[TestFixture]
    public class TrickTestAsync
    {
        //[Test]
        public void test()
        {
            var trick = new Trick(Suit.NoTrumps);
            Task.Factory.StartNew(() =>
            {
                trick.West(Suit.Hearts, Rank.Ace);
            });
            Task.Factory.StartNew(() =>
            {
                trick.East(Suit.Hearts, Rank.King);
            });
            Task.Factory.StartNew(() =>
            {
                trick.North(Suit.Hearts, Rank.Queen);
            });
            Task.Factory.StartNew(()=>
            {
                trick.South(Suit.Hearts, Rank.Jack);
            });
            Assert.AreEqual(PlayerPosition.West, trick.Winner);
        }
    }
}