using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.User.Events
{
    public class UserDeleted: Event
    {
        public string DeletedByUserId { get; set; }
    }
}