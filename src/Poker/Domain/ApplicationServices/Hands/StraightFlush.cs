using System.Linq;

namespace Poker.Domain.ApplicationServices.Hands
{
    public class StraightFlush : BasePokerHand
    {
        public override string Name
        {
            get
            {
                return PokerNames.StraightFlush;
            }
        }

        public override int Score
        {
            get
            {
                return (int)PokerScores.StraightFlush;
            }
        }

        public override bool IsPresent()
        {
            var straight = new Straight();
            var flush = new Flush();
            straight.SetCards(Cards.ToList());
            flush.SetCards(Cards.ToList());
            return straight.IsPresent() && flush.IsPresent();
        }

        protected override int CompareWithSame(IPokerHand other)
        {
            throw new System.NotImplementedException();
        }
    }
}