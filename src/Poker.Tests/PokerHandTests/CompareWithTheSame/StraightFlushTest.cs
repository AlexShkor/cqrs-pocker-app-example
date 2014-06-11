using NUnit.Framework;
using Poker.Domain.ApplicationServices.Hands;
using Poker.Domain.Data;

namespace Poker.Tests.PokerHandTests.CompareWithTheSame
{
    [TestFixture]
    public class StraightFlushTest
    {
        [Test]
        public void AceAsOne()
        {
            var hand1 = new StraightFlush();
            hand1.SetCards(Cards.New()
                .Ace(Suit.Clubs)
                .Two(Suit.Clubs)
                .Three(Suit.Clubs)
                .Four(Suit.Clubs)
                .Five(Suit.Clubs)
                .Nine(Suit.Spades)
                .Ten(Suit.Spades));
            var hand2 = new StraightFlush();
            hand2.SetCards(Cards.New()
                .Two(Suit.Clubs)
                .Three(Suit.Clubs)
                .Four(Suit.Clubs)
                .Five(Suit.Clubs)
                .Six(Suit.Clubs)
                .Nine(Suit.Spades)
                .Jack(Suit.Spades));
            Assert.IsTrue(hand1.IsPresent());
            Assert.IsTrue(hand2.IsPresent());
            Assert.Less(hand1, hand2);
        }
    }
}