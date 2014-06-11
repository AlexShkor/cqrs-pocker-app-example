using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices
{
    public abstract class BasePokerHand: IPokerHand, IComparable
    {
        protected BasePokerHand()
        {
            Cards = new List<Card>();
            HandCards = new List<Card>();
        }

        public int CompareTo(IPokerHand other)
        {
            if (Score == other.Score)
            {
                return CompareWithSame(other);
            }
            return Score.CompareTo(other.Score);
        }

        public IReadOnlyList<Card> Cards { get; private set; }

        public List<Card> HandCards { get; private set; }

        public void SetCards(IList<Card> cards)
        {
            Cards = new ReadOnlyCollection<Card>(cards);
        }

        
        public abstract string Name { get; }

        public abstract int Score { get; }

        public abstract bool IsPresent();

        protected abstract int CompareWithSame(IPokerHand other);

        public int CompareKickers(IPokerHand other)
        {
            var cards = Cards.Except(HandCards).Select(x => x.Rank).OrderByDescending(x => x).ToList();
            var otherCards = other.Cards.Except(other.HandCards).Select(x => x.Rank).OrderByDescending(x => x).ToList();
            
            for (int i = 0; i < 5 - HandCards.Count; i++)
            {
                var kickerResult = cards[i].CompareTo(otherCards[i]);
                if (kickerResult != 0)
                {
                    return kickerResult;
                }
            }
            return 0;
        }

        public int CompareHandMaxRank(IPokerHand other)
        {
            return HandCards.Max(x => x.Rank).CompareTo(other.HandCards.Max(x => x.Rank));
        }

        public int CompareStraight(IPokerHand other)
        {
            return GetStraightHighestRank(this).CompareTo(GetStraightHighestRank(other));
        }

        private static Rank GetStraightHighestRank(IPokerHand hand)
        {
            var ranks = new List<Rank>(hand.HandCards.Select(x=> x.Rank));
            if (ranks.Contains(Rank.Ace) && ranks.Contains(Rank.Two))
            {
                ranks.Remove(Rank.Ace);
            }
            return ranks.Max();
        }

        public int CompareTo(object obj)
        {
            return CompareStraight((IPokerHand) obj);
        }
    }
}