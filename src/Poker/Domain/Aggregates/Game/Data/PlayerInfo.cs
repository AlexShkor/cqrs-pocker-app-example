namespace Poker.Domain.Aggregates.Game.Data
{
    public class PlayerInfo
    {
        public PlayerInfo(GamePlayer gamePlayer)
        {
            UserId = gamePlayer.UserId;
            Position = gamePlayer.Position;
        }

        public PlayerInfo()
        {
            
        }

        public PlayerInfo(int position, string userId)
        {
            Position = position;
            UserId = userId;
        }

        public string UserId { get; set; }

        public int Position { get; set; }
    }
}