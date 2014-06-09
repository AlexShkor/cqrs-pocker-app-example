using System.Linq;

namespace Poker.Domain.ApplicationServices.Hands
{
    public class FullHouse : BasePokerHand
    {
        public override string Name
        {
            get
            {
                return PokerNames.FullHouse;
            }
        }

        public override int Score
        {
            get
            {
                return (int)PokerScores.FullHouse;
            }
        }

        
        public override bool IsPresent()
        {
            var set = new Set();
            var pairSet = new OnePair();
            set.SetCards(Cards.ToList());
            pairSet.SetCards(Cards.ToList());

            var isPresent = set.IsPresent() && pairSet.IsPresent();

            if (isPresent)
            {
                HandCards.AddRange(set.HandCards);
                HandCards.AddRange(pairSet.HandCards);
            }

            return isPresent;
        }

        protected override int CompareWithSame(IPokerHand other)
        {
            var result = HandCards.First().Rank.CompareTo(other.HandCards.First().Rank);

            if (result == 0)
            {
                result = HandCards.Last().Rank.CompareTo(other.HandCards.Last().Rank);
            }

            return result;
        }
    }
}