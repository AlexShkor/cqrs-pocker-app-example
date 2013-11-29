using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices
{
    public abstract class BasePokerSet: IPokerSet
    {
        protected BasePokerSet()
        {
            Cards = new List<Card>();
        }

        public int CompareTo(IPokerSet other)
        {
            if (Score == other.Score)
            {
                return CompareWithSame(other);
            }
            return Score.CompareTo(other.Score);
        }

        public IReadOnlyList<Card> Cards { get; private set; }

        public void SetCards(IList<Card> cards)
        {
            Cards = new ReadOnlyCollection<Card>(cards);
        }

        public abstract string Name { get; }

        public abstract int Score { get; }

        public abstract bool IsPresent();

        protected abstract int CompareWithSame(IPokerSet other);
    }
}