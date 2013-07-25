using System;
using System.Collections.Generic;

namespace AKQ.Domain
{
    public class Pack
    {
        private const int MAX_CARDS_COUNT = 52;

        private static readonly Random Random = new Random((int)(DateTime.Now.Ticks/52));

        private readonly List<Card> _cards = new List<Card>(MAX_CARDS_COUNT); 

        public bool IsEmpty
        {
            get { return _cards.Count == 0; }
        }

        public Pack()
        {
            foreach (var suit in Suit.GetAll())
            {
                foreach (var rank in Rank.GetAll())
                {
                    _cards.Add(new Card(suit,rank));
                }
            }
        }

        public Pack(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
            {
               if (_cards.Contains(card))
               {
                   throw new ArgumentException("Pack can't be created with duplicated cards.", "cards");
               }
                _cards.Add(card);
            }
        }

        public int CardsCount()
        {
            return _cards.Count;
        }

        public Card TakeRandom()
        {
            if (_cards.Count == 0)
            {
                throw new InvalidOperationException("Can't take a card when pack is empty.");
            }
            var card = _cards[Random.Next(_cards.Count)];
            _cards.Remove(card);
            return card;
        }
    }

    public class GameException: Exception
    {
    }
}
