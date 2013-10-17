
using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Game.Events
{
    public class PlayerLeft: Event
    {
         public string UserId { get; set; }
    }
}