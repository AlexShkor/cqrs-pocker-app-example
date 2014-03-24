using Poker.Domain.Data;

namespace Poker.Domain.Aggregates.Game.Data
{
    public class PlayerCard
    {
        public string UserId { get; set; }
        public int Position { get; set; }
        public Card Card { get; set; }
    }
}