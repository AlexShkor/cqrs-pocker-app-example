using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Commands
{
    public class CheckGame : Command
    {
        public string GameId { get; set; }

        public string UserId { get; set; }
    }
}