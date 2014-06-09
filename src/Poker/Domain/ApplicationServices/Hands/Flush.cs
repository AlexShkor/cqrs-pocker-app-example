using System.Linq;

namespace Poker.Domain.ApplicationServices.Hands
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
            var flush = Cards.GroupBy(x => x.Suit).FirstOrDefault(x => x.Count() >= 5);
            if (flush != null)
            {
                HandCards.AddRange(flush.Take(5));
                return true;
            }
            return false;
        }

        protected override int CompareWithSame(IPokerHand other)
        {
            return CompareHandMaxRank(other);
        }
    }
}