using System.Linq;

namespace Poker.Domain.ApplicationServices.Hands
{
    public class HighCard : BasePokerHand
    {
        public override string Name
        {
            get
            {
                return PokerNames.HighCard;
            }
        }

        public override int Score
        {
            get
            {
                return (int)PokerScores.HighCard;
            }
        }

        protected override int CompareWithSame(IPokerHand other)
        {
            return CompareKickers(other);
        }

        public override bool IsPresent()
        {
            HandCards.Add(Cards.OrderByDescending(x=> x.Rank).First());
            return true;
        }
    }
}