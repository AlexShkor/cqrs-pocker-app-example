using System.Collections.Generic;
using System.Linq;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices.Combinations
{
    public class TwoPairs : BasePokerSet
    {
        public override string Name
        {
            get
            {
                return PokerNames.TwoPairs;
            }
        }

        public override int Score
        {
            get
            {
                return (int)PokerScores.TwoPairs;
            }
        }

        public override bool IsPresent()
        {
            return Cards.GroupBy(x => x.Rank).Count(x => x.Count() == 2) == 2;
        }

        protected override int CompareWithSame(IPokerSet other)
        {
            throw new System.NotImplementedException();
        }
    }
}