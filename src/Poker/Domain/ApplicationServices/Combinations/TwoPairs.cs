using System.Collections.Generic;
using System.Linq;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices.Combinations
{
    public class TwoPairs : BasePokerHand
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
            var pairs = Cards.OrderByDescending(c => c.Rank).GroupBy(x => x.Rank).Where(g => g.Count() == 2);

            if (pairs.Count() >= 2)
            {
                int count = 0;
                foreach (var pair in pairs)
                {
                    count++;
                    if (count <= 2)
                        HandCards.AddRange(pair);
                }
            }

            return pairs.Count() >= 2;
        }

        protected override int CompareWithSame(IPokerHand other)
        {
            var ranks = HandCards.OrderBy(c => c.Rank).GroupBy(c => c.Rank).Select(group => group.First()).Select(c => c.Rank).ToList();
            var otherRanks = other.HandCards.OrderBy(c => c.Rank).GroupBy(c => c.Rank).Select(group => group.First()).Select(c => c.Rank).ToList();

            var result = ranks.First().CompareTo(otherRanks.First());

            if (result == 0)
            {
                return CompareKickers(other);
            }

            return result;
        }
    }
}