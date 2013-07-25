using System;
using System.Collections.Generic;
using System.Linq;

namespace AKQ.Domain
{
    [Serializable]
    public class Hand
    {
        public PlayerPosition Position { get; private set; }

        private readonly Dictionary<Suit, List<Card>> _suits;

        public Hand(PlayerPosition position, IEnumerable<Card> cards)
        {
            Position = position;
            _suits = new Dictionary<Suit, List<Card>>
            {
                {Suit.Spades, new List<Card>()},
                {Suit.Hearts, new List<Card>()},
                {Suit.Diamonds, new List<Card>()},
                {Suit.Clubs, new List<Card>()},
            };
            foreach (var card in cards)
            {
                _suits[card.Suit].Add(card);
            }
        }

        public bool HasSuitCard(Suit suit)
        {
            return suit != Suit.NoTrumps && _suits[suit].Any();
        }

        public void RemoveCard(Card card)
        {
            _suits[card.Suit].Remove(card);
        }

        public List<Card> GetCards(Suit spades)
        {
            return _suits[spades];
        }

        public void Remove(Card card)
        {
            _suits[card.Suit].Remove(card);
        }

        public IEnumerable<Card> GetAll()
        {
            return _suits.SelectMany(suit => suit.Value);
        }

        public bool HasCard(Card card)
        {
            return _suits[card.Suit].Contains(card);
        }
    }
}