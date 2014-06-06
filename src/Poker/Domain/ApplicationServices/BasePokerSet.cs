using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices
{
    public abstract class BasePokerHand: IPokerHand
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
    }
}