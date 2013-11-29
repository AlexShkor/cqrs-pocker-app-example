using System.Collections.Generic;
using System.Linq;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices.Combinations
{
    public class Quads : BasePokerSet
    {
        public override string Name
        {
            get
            {
                return PokerNames.Quads;
            }
        }

        public override int Score
        {
            get
            {
                return (int)PokerScores.Quads;
            }
        }

        public override bool IsPresent()
        {
            return Cards.GroupBy(x => x.Rank).Any(x => x.Count() == 4);
        }

        protected override int CompareWithSame(IPokerSet other)
        {
            throw new System.NotImplementedException();
        }
    }
}