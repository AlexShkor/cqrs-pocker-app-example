using System.Collections.Generic;
using System.Linq;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices.Combinations
{
    public class OnePair : BasePokerSet
    {
        public override string Name 
        {
            get { return PokerNames.OnePair; }
        }

        public override int Score
        {
            get { return (int) PokerScores.OnePair; }
        }

        public override bool IsPresent()
        {
            return Cards.GroupBy(x => x.Rank).Any(x => x.Count() == 2);
        }

        protected override int CompareWithSame(IPokerSet other)
        {
            throw new System.NotImplementedException();
        }
    }
}