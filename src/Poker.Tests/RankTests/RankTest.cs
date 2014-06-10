using NUnit.Framework;
using Poker.Domain.Data;

namespace Poker.Tests
{
    [TestFixture]
    public class RankTest
    {
        [Test]
        public void Mapping()
        {
            Assert.AreEqual(Rank.FromString("2"), Rank.Two);
            Assert.AreEqual(Rank.FromString("3"), Rank.Three);
            Assert.AreEqual(Rank.FromString("4"), Rank.Four);
            Assert.AreEqual(Rank.FromString("5"), Rank.Five);
            Assert.AreEqual(Rank.FromString("6"), Rank.Six);
            Assert.AreEqual(Rank.FromString("7"), Rank.Seven);
            Assert.AreEqual(Rank.FromString("8"), Rank.Eight);
            Assert.AreEqual(Rank.FromString("9"), Rank.Nine);
            Assert.AreEqual(Rank.FromString("10"), Rank.Ten);
            Assert.AreEqual(Rank.FromString("T"), Rank.Ten);
            Assert.AreEqual(Rank.FromString("J"), Rank.Jack);
            Assert.AreEqual(Rank.FromString("Q"), Rank.Queen);
            Assert.AreEqual(Rank.FromString("K"), Rank.King);
            Assert.AreEqual(Rank.FromString("A"), Rank.Ace);
        }

        [Test]
        public void Equality()
        {
            Assert.IsTrue(Rank.FromString("2") == Rank.Two);
        }

        [Test]
        public void Comparation()
        {
            Assert.IsTrue(Rank.Two < Rank.Three);
            Assert.IsTrue(Rank.Two <= Rank.FromString("2"));
            Assert.IsTrue(Rank.Four > Rank.Two);
            Assert.IsTrue(Rank.Ace >= Rank.FromString("A"));
        }
    }
}