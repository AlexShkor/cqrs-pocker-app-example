using System.Collections.Generic;
using System.Linq;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices.Combinations
{
    public class RoyalFlush : BasePokerSet
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
            return Cards.Where(x=> royal.Contains(x.Rank)).GroupBy(x=> x.Suit).Count() == 1;
        }

        protected override int CompareWithSame(IPokerSet other)
        {
            throw new System.NotImplementedException();
        }
    }
}