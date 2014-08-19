using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.User.Commands
{
    public class SetProfileAvatar: Command
    {
        public string AvatarId { get; set; }
    }
}