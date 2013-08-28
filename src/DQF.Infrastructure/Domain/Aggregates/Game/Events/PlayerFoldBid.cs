using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Events
{
    public class PlayerFoldBid : Event
    {
        public string GameId { get; set; }
        public string UserId { get; set; }
    }
}