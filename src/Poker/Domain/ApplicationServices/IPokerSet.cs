using System;
using System.Collections.Generic;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices
{
    public interface IPokerSet : IComparable<IPokerSet>
    {
        IReadOnlyList<Card> Cards { get; }

        string Name { get; }

        int Score { get; }

        void SetCards(IList<Card> cards);

        bool IsPresent();
    }
}