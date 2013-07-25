using AKQ.Domain.Services;

namespace AKQ.Domain.UserEvents
{
    public class PlayerJoined : GameEvent
    {
        public PlayerPosition Position { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
    }
}