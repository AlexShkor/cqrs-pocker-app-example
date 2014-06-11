using NUnit.Framework;
using Poker.Domain.ApplicationServices.Hands;
using Poker.Domain.Data;

namespace Poker.Tests.PokerHandTests.CompareWithTheSame
{
    [TestFixture]
    public class StraightTest
    {
        [Test]
        public void AceAsOne()
        {
            var hand1 = new Straight();
            hand1.SetCards(Cards.New()
                .Ace(Suit.Clubs)
                .Two(Suit.Clubs)
                .Three(Suit.Diamonds)
                .Four(Suit.Diamonds)
                .Five(Suit.Hearts)
                .Nine(Suit.Spades)
                .Ten(Suit.Spades));
            var hand2 = new Straight();
            hand2.SetCards(Cards.New()
                .Two(Suit.Clubs)
                .Three(Suit.Diamonds)
                .Four(Suit.Diamonds)
                .Five(Suit.Hearts)
                .Six(Suit.Hearts)
                .Nine(Suit.Spades)
                .Jack(Suit.Spades));
            Assert.IsTrue(hand1.IsPresent());
            Assert.IsTrue(hand2.IsPresent());
            Assert.Less(hand1, hand2);
        }
    }
}