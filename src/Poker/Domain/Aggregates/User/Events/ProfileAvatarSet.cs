using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.User.Events
{
    public class ProfileAvatarSet: Event
    {
        public string AvatarId { get; set; }
    }
}