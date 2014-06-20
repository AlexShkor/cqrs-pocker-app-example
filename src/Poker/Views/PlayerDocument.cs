using System.Collections.Generic;
using Poker.Domain.Data;

namespace Poker.Views
{
    public class PlayerDocument
    {
        public int Position { get; set; }
        public long Cash { get; set; }
        public List<Card> Cards { get; set; }
        public long Bid { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public bool CurrentTurn { get; set; }
        public bool IsSmallBlind { get; set; }
        public bool IsBigBlind { get; set; }

        

        public PlayerDocument()
        {
            Cards = new List<Card>();
        }
    }
}