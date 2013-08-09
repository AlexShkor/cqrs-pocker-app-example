
using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Events
{
    public class PlayerLeft: Event
    {
         public string UserId { get; set; }
    }
}