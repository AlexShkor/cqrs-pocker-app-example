using System;
using System.Collections.Generic;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices
{
    public interface IPokerHand : IComparable<IPokerHand>
    {
        IReadOnlyList<Card> Cards { get; }

        List<Card> HandCards { get; }

        string Name { get; }

        int Score { get; }

        void SetCards(IList<Card> cards);

        bool IsPresent();
    }
}