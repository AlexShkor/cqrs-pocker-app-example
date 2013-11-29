using System.Linq;

namespace Poker.Domain.ApplicationServices.Combinations
{
    public class HighCard : BasePokerSet
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

        protected override int CompareWithSame(IPokerSet other)
        {
            return Cards.Max(x => x.Rank).CompareTo(other.Cards.Max(x => x.Rank));
        }

        public override bool IsPresent()
        {
            return true;
        }
    }
}