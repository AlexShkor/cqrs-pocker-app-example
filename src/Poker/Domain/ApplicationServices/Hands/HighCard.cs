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
            return Cards.Max(x => x.Rank).CompareTo(other.Cards.Max(x => x.Rank));
        }

        public override bool IsPresent()
        {
            return true;
        }
    }
}