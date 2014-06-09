using System.Linq;

namespace Poker.Domain.ApplicationServices.Hands
{
    public class OnePair : BasePokerHand
    {
        public override string Name
        {
            get { return PokerNames.OnePair; }
        }

        public override int Score
        {
            get { return (int)PokerScores.OnePair; }
        }

        public override bool IsPresent()
        {
            var pair = Cards.GroupBy(x => x.Rank).FirstOrDefault(x => x.Count() == 2);
            if (pair != null)
            {
                HandCards.AddRange(pair);
            }
            return pair != null;
        }

        protected override int CompareWithSame(IPokerHand other)
        {
            var result = CompareHandMaxRank(other);
            if (result == 0)
            {
                return CompareKickers(other);
            }
            return result;
        }
    }
}