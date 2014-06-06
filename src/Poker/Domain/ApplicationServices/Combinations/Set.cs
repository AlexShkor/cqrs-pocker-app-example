using System.Collections.Generic;
using System.Linq;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices.Combinations
{
    public class Set : BasePokerHand
    {
        public override string Name
        {
            get
            {
                return PokerNames.Set;
            }
        }

        public override int Score
        {
            get
            {
                return (int)PokerScores.Set;
            }
        }

        public override bool IsPresent()
        {
            return Cards.GroupBy(x => x.Rank).Any(x => x.Count() == 3);
        }

        protected override int CompareWithSame(IPokerHand other)
        {
            throw new System.NotImplementedException();
        }
    }
}