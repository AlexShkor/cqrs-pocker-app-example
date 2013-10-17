using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.User.Events
{
    public class PasswordChanged : Event
    {
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool WasChangedByAdmin { get; set; }
    }
}