namespace AKQ.Domain
{
    public abstract class GameCommand
    {
        public string GameId { get; set; }
        public string UserId { get; set; }
    }
}