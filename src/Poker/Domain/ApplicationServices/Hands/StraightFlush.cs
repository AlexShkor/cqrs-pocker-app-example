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
            straight.SetCards(Cards.ToList());
            var isStraight = straight.IsPresent();
            var flush = new Flush();
            flush.SetCards(Cards.ToList());
            var isFlush = flush.IsPresent();
            HandCards.AddRange(straight.HandCards.Intersect(flush.HandCards));
            return  isStraight && isFlush && HandCards.Count == 5;
        }

        protected override int CompareWithSame(IPokerHand other)
        {
            throw new System.NotImplementedException();
        }
    }
}