using NUnit.Framework;
using Poker.Domain.Data;

namespace Poker.Tests.PokerSetTests
{
    public static class CardExt
    {
        public static void AssertRank(this Card card, Rank rank)
        {
            Assert.AreEqual(rank, card.Rank);
        }
    }
}