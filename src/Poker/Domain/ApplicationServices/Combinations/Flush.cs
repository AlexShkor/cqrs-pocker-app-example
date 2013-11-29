using System.Collections.Generic;
using System.Linq;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices.Combinations
{
    public class Flush : BasePokerSet
    {
        public override string Name
        {
            get
            {
                return PokerNames.Flush;
            }
        }

        public override int Score
        {
            get
            {
                return (int)PokerScores.Flush;
            }
        }

        protected override int CompareWithSame(IPokerSet other)
        {
            return 0;
        }

        public override bool IsPresent()
        {
            return Cards.GroupBy(x => x.Suit).Any(x => x.Count() >= 5);
        }
    }
}