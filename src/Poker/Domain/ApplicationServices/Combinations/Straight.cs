using System.Collections.Generic;
using System.Linq;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices.Combinations
{
    public class Straight : BasePokerHand
    {
        public override string Name
        {
            get
            {
                return PokerNames.Straight;
            }
        }

        public override int Score
        {
            get
            {
                return (int)PokerScores.Straight;
            }
        }

        public override bool IsPresent()
        {
            var ordered = Cards.OrderBy(x => x.Rank).ToList();
            var countInOrder = 1;
            var previousRank = ordered[0].Rank;
            for (int i = 1; i < ordered.Count; i++)
            {
                var card = ordered[i];
                if (card.Rank.Score == previousRank.Score + 1)
                {
                    countInOrder += 1;
                }
                else
                {
                    countInOrder = 1;
                }
                if (countInOrder == 5)
                {
                    return true;
                }
                previousRank = card.Rank;
            }
            return false;
        }

        protected override int CompareWithSame(IPokerHand other)
        {
            throw new System.NotImplementedException();
        }
    }
}