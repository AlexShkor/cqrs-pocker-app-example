using System.Collections.Generic;
using System.Linq;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices.Combinations
{
    public class RoyalFlush : BasePokerHand
    {
        public override string Name
        {
            get
            {
                return PokerNames.RoyalFlush;
            }
        }

        public override int Score
        {
            get
            {
                return (int)PokerScores.RoyalFlush;
            }
        }

        public override bool IsPresent()
        {
            var royal = new List<Rank>
            {
                Rank.Ace,
                Rank.King,
                Rank.Queen,
                Rank.Jack,
                Rank.Ten
            };
            var straightFlash = new StraightFlush();
            straightFlash.SetCards(Cards.ToList());
            return Cards.Count(x => royal.Contains(x.Rank)) == 5 && straightFlash.IsPresent();
        }

        protected override int CompareWithSame(IPokerHand other)
        {
            throw new System.NotImplementedException();
        }
    }
}