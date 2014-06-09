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
            return Cards.GroupBy(x => x.Rank).Any(x => x.Count() == 4);
        }

        protected override int CompareWithSame(IPokerHand other)
        {
            throw new System.NotImplementedException();
        }
    }
}