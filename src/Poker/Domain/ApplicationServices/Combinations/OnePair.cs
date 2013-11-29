using System.Collections.Generic;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices.Combinations
{
    public class OnePair : BasePokerSet
    {
        public override string Name 
        {
            get { return PokerNames.OnePair; }
        }

        public override int Score
        {
            get { return (int) PokerScores.OnePair; }
        }

        public override bool IsPresent()
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                for (int j = i + 1; j < Cards.Count; j++)
                {
                    if (Cards[i].Rank == Cards[j].Rank)
                    {
                        return true;
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