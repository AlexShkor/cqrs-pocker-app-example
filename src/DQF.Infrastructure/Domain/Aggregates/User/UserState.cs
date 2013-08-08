using PAQK.Domain.Aggregates.User.Events;
using PAQK.Platform.Domain;

namespace PAQK.Domain.Aggregates.User
{
    public sealed class UserState: AggregateState
    {
        public string Id { get; private set; }
        public bool UserWasDeleted { get; set; }

        public UserState()
        {
            On((UserCreated e) => Id = e.Id);
            On((UserDeleted e) => UserWasDeleted = true);
        }
    }
}