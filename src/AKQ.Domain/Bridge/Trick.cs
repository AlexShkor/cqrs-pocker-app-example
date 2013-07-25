using System;
using System.Collections.Generic;
using System.Linq;
using AKQ.Domain.Utils;

namespace AKQ.Domain
{
    public class Trick
    {
        private readonly Suit _trumpSuit;
        public Dictionary<PlayerPosition, Card> PlayedCards { get; private set; }

        public PlayerPosition Winner
        {
            get
            {
                var highestTrump = PlayedCards.Where(c => c.Value.Suit == _trumpSuit).OrderByDescending(c => c.Value.Rank.Score);
                var highestInTrickDealerSuit = PlayedCards.Where(c => c.Value.Suit == LedSuit).OrderByDescending(c => c.Value.Rank.Score);
                return highestTrump.Any() ? highestTrump.First().Key : highestInTrickDealerSuit.First().Key;
            }
        }

        public bool TrickIsComplete
        {
            get { return PlayedCards.Count == 4; }
        }

        public bool IsEmpty
        {
            get { return PlayedCards.Count == 0; }
        }

        public Suit LedSuit { get; private set; }

        public Trick(Suit trump)
        {
            _trumpSuit = trump;
            LedSuit = Suit.NoTrumps;
            PlayedCards = new Dictionary<PlayerPosition, Card>();
        }

        public void AddCard(PlayerPosition player, Card card)
        {
            Guard.Against<ArgumentException>(PlayedCards.ContainsKey(player), "Player {0} has already played in this trick", player.ToString());
            if (IsEmpty)
            {
                LedSuit = card.Suit;
            }
            PlayedCards[player] = card;
        }

        public Card? GetCard(PlayerPosition pos)
        {
            if (PlayedCards.ContainsKey(pos))
            {
                return PlayedCards[pos];
            }
            return null;
        }
    }
}