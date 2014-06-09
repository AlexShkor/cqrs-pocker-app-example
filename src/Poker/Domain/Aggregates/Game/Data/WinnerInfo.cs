namespace Poker.Domain.Aggregates.Game.Data
{
    public class WinnerInfo
    {
        public string UserId { get; set; }

        public int Position { get; set; }

        public long Amount { get; set; }

        public int Order { get; set; }

        public int HandScore { get; set; }

        public WinnerInfo()
        {
            
        }

        public WinnerInfo(GamePlayer player, long prize)
            : this(player.UserId, player.Position, prize, 1, 0)
        {
        }

        public WinnerInfo(string userId, int position, long prize, int order, int score)
        {
            UserId = userId;
            Position = position;
            Amount = prize;
            Order = order;
            HandScore = score;
        }
    }
}