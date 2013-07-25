using System.Collections.Generic;
using NUnit.Framework;
using AKQ.Domain;
namespace AKQ.Tests
{
    [TestFixture]
    public class BidTest
    {
        [Test]
         public void test()
         {
             Assert.AreEqual(new Bid(3,Suit.Spades),Bid.FromString("3S"));
             Assert.AreEqual(new Bid(1,Suit.Hearts),Bid.FromString("1H"));
             Assert.AreEqual(new Bid(0,Suit.Diamonds),Bid.FromString("0D"));
             Assert.AreEqual(new Bid(6,Suit.Clubs),Bid.FromString("6C"));
         }
    }
}