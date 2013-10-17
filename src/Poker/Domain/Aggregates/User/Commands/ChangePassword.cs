using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.User.Commands
{
    public class ChangePassword: Command
    {
        public string NewPassword { get; set; }
        public bool IsChangedByAdmin { get; set; }
    }
}