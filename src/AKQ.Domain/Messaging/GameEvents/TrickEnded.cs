using AKQ.Domain.Services;

namespace AKQ.Domain.UserEvents
{
    public class TrickEnded : GameEvent
    {
        public PlayerPosition Winner { get; set; }
        public int TrickNumber { get; set; }
        public Trick Trick { get; set; }
    }
}