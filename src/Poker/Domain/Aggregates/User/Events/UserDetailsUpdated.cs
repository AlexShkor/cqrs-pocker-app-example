using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.User.Events
{
    public class UserDetailsUpdated: Event
    {
        public string UserName { get; set; }
    }
}