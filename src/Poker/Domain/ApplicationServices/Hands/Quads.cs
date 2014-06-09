using System.Linq;

namespace Poker.Domain.ApplicationServices.Hands
{
    public class Quads : BasePokerHand
    {
        public override string Name
        {
            get
            {
                return PokerNames.Quads;
            }
        }

        public override int Score
        {
            get
            {
                return (int)PokerScores.Quads;
            }
        }

        public override bool IsPresent()
        {
            var quads = Cards.GroupBy(x => x.Rank).Where(x => x.Count() == 4);

            if (quads.Count() == 1)
            {
                HandCards.AddRange(quads.Single());
            }

            return quads.Count() == 1;
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