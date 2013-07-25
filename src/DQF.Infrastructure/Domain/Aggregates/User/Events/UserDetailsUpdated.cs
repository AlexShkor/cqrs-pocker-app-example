using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.User.Events
{
    public class UserDetailsUpdated: Event
    {
        public string UserName { get; set; }
    }
}