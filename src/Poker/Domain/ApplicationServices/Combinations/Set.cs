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
            var sets = Cards.OrderByDescending(c => c.Rank).GroupBy(x => x.Rank).Where(x => x.Count() == 3);

            if (sets.Any())
            {
                HandCards.AddRange(sets.First());
                return true;
            }

            return false;
        }


        protected override int CompareWithSame(IPokerHand other)
        {
            var result = HandCards.First().Rank.CompareTo(other.HandCards.First().Rank);

            if (result == 0)
            {
                return CompareKickers(other);
            }

            return result;
        }
    }
}