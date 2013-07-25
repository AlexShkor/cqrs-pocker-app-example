using AKQ.Domain.Services;

namespace AKQ.Domain.UserEvents
{
    public class PlayerTurn : GameEvent
    {
        public PlayerPosition Player { get; set; }
        public Suit LedSuit { get; set; }
        public bool IsAI { get; set; }
        public string RoboBridgePBN { get; set; }
        public PlayerPosition CurrentOrPartner { get; set; }
        public bool IsNextTrick { get; set; }
        public string UserId { get; set; }

        public int TrickNumber { get; set; }

        public bool UseDDS { get; set; }

        public string GamePBN { get; set; }
    }
}