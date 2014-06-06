using System.Collections.Generic;
using System.Linq;
using Poker.Domain.Data;


namespace Poker.Domain.ApplicationServices.Combinations
{
    public class Flush : BasePokerHand
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

        public override bool IsPresent()
        {
            var flush = Cards.OrderByDescending(c => c.Rank).GroupBy(x => x.Suit).Where(x => x.Count() >= 5);

            if (flush.Count() == 1)
            {
                foreach (var f in flush)
                {
                    HandCards.AddRange(f.Take(5));
                }
            }

            return flush.Count() == 1;
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