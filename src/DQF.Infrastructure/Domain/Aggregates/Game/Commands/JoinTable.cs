using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Commands
{
    public class JoinTable: Command
    {
        public string UserId { get; set; }

        public long Cash { get; set; }
    }
}