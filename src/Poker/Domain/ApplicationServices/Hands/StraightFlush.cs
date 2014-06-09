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
            if (!isFlush || !isStraight)
            {
                return false;
            }
            var suit = flush.HandCards.Select(x => x.Suit).First();
            var ranks = straight.HandCards.Select(x => x.Rank).ToArray();
            var handCards = straight.Cards.Where(x => ranks.Contains(x.Rank) && x.Suit == suit);
            HandCards.AddRange(handCards);
            return HandCards.Count == 5;
        }

        protected override int CompareWithSame(IPokerHand other)
        {
            return CompareHandMaxRank(other);
        }
    }
}