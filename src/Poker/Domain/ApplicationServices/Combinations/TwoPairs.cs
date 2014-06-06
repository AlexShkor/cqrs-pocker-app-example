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

        //public override bool IsPresent()
        //{
        //    return Cards.GroupBy(x => x.Rank).Count(x => x.Count() == 2) == 2;
        //}

        public override bool IsPresent()
        {
            //var groups = Cards.OrderByDescending(c => c.Rank).GroupBy(x => x.Rank).Where(g => g.Count() == 2);
            var groups = Cards.OrderBy(c => c.Rank).GroupBy(x => x.Rank).Where(g => g.Count() == 2);

            if (groups.Count() == 2)
            {
                foreach (var group in groups)
                {
                    HandCards.AddRange(group);
                }
            }

            return groups.Count() == 2;

        }

        protected override int CompareWithSame(IPokerHand other)
        {
            var ranks = HandCards.GroupBy(c => c.Rank).Select(group => group.First()).Select(c => c.Rank).ToList();
            var otherRanks = other.HandCards.GroupBy(c => c.Rank).Select(group => group.First()).Select(c => c.Rank).ToList();

            var result = ranks.First().CompareTo(otherRanks.First());

            if (result == 0)
            {
                return CompareKickers(other);
            }

            return result;
            throw new System.NotImplementedException();
        }

        //public override bool IsPresent()
        //{
        //    var group = Cards.GroupBy(x => x.Rank).FirstOrDefault(x => x.Count() == 2);
        //    if (group != null)
        //    {
        //        HandCards.AddRange(group);
        //    }
        //    return group != null;
        //}

        //protected override int CompareWithSame(IPokerHand other)
        //{
        //    var result = HandCards.First().Rank.CompareTo(other.HandCards.First().Rank);
        //    if (result == 0)
        //    {
        //        return CompareKickers(other);
        //    }
        //    return result;
        //}


    }
}