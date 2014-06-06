using System;
using System.Collections.Generic;
using System.Linq;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices.Combinations
{
    public class OnePair : BasePokerHand
    {
        public override string Name
        {
            get { return PokerNames.OnePair; }
        }

        public override int Score
        {
            get { return (int)PokerScores.OnePair; }
        }

        public override bool IsPresent()
        {
            var group = Cards.GroupBy(x => x.Rank).FirstOrDefault(x => x.Count() == 2);
            if (group != null)
            {
                HandCards.AddRange(group);
            }
            return group != null;
        }

        protected override int CompareWithSame(IPokerHand other)
        {
            var result = HandCards.First().Rank.CompareTo(other.HandCards.First().Rank);
            if (result == 0)
            {
                return CompareKickers(other);
            }
            return result;
        }
    }
}