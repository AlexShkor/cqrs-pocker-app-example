using System.Collections.Generic;
using Poker.Domain.Data;

namespace Poker.Domain.Aggregates.Game.Data
{
    public class GamePlayer
    {
        public string UserId { get; set; }
        public int Position { get; set; }
        public List<Card> Cards { get; set; }
        public bool Fold { get; set; }
        public long Bid { get; set; }
        public bool AllIn { get; set; }

        public GamePlayer()
        {
            Cards = new List<Card>();
        }
    }
}