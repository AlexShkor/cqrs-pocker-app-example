using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Commands
{
    public class CheckBid : Command
    {
        public string UserId { get; set; }
    }
}