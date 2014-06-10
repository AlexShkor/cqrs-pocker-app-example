using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Poker.Domain.ApplicationServices;
using Poker.Domain.ApplicationServices.Hands;
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
            detector.AddPlayer("me1", Cards.TwoPairsJacksFives());
            detector.AddPlayer("me2", Cards.TwoPairsJacksSixes());
            var winners = detector.GetWinners(100).ToList();
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("me2", winners[0].UserId);
            Assert.AreEqual(100, winners[0].Prize);
            Assert.AreEqual(typeof(TwoPairs), winners[0].PokerHand.GetType());
        }
    }
}