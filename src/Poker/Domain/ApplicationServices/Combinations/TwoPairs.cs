using System.Collections.Generic;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices.Combinations
{
    public class TwoPairs : BasePokerSet
    {
        public override string Name
        {
            get
            {
                return PokerNames.TwoPairs;
            }
        }

        public override int Score
        {
            get
            {
                return (int)PokerScores.TwoPairs;
            }
        }

        public override bool IsPresent()
        {
            var pairs = new List<Rank>();
            for (int i = 0; i < Cards.Count; i++)
            {
                for (int j = i + 1; j < Cards.Count; j++)
                {
                    if (Cards[i].Rank == Cards[j].Rank && !pairs.Contains(Cards[i].Rank))
                    {
                        pairs.Add(Cards[i].Rank);
                        if (pairs.Count == 2)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        protected override int CompareWithSame(IPokerSet other)
        {
            throw new System.NotImplementedException();
        }
    }
}