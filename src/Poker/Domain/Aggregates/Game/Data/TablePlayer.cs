namespace Poker.Domain.Aggregates.Game.Data
{
    public class TablePlayer
    {
        public string UserId { get; set; }

        public long Cash { get; set; }

        public int Position { get; set; }
    }
}