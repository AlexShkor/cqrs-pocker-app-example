using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.User.Events
{
    public class UserDeleted: Event
    {
        public string DeletedByUserId { get; set; }
    }
}