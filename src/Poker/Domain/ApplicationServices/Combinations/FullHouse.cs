using System.Collections.Generic;
using System.Linq;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices.Combinations
{
    public class FullHouse : BasePokerSet
    {
        public override string Name
        {
            get
            {
                return PokerNames.FullHouse;
            }
        }

        public override int Score
        {
            get
            {
                return (int)PokerScores.FullHouse;
            }
        }

        public override bool IsPresent()
        {
            var set = new Set();
            var pairSet = new OnePair();
            set.SetCards(Cards.ToList());
            pairSet.SetCards(Cards.ToList());
            return set.IsPresent() && pairSet.IsPresent();
        }

        protected override int CompareWithSame(IPokerSet other)
        {
            throw new System.NotImplementedException();
        }
    }
}