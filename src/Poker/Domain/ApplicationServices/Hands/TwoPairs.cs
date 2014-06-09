using System.Linq;

namespace Poker.Domain.ApplicationServices.Hands
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
            var result = CompareHandMaxRank(other);
            if (result == 0)
            {
                result = HandCards.Min(x => x.Rank).CompareTo(other.HandCards.Min(x => x.Rank));
            }
            if (result == 0)
            {
                result = CompareKickers(other);
            }
            return result;
        }
    }
}