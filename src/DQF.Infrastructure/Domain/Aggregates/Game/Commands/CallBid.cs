using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Commands
{
    public class CallBid : Command
    {
        public string UserId { get; set; }
    }
}