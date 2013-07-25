namespace AKQ.Domain.Services
{
    public abstract class GameEvent
    {
        public int Version { get; set; }
        public string GameId { get; set; }
    }
}