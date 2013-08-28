using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Commands
{
    public class JoinTable: Command
    {
        public int Position { get; set; }

        public string UserId { get; set; }

        public long Cash { get; set; }
    }
}