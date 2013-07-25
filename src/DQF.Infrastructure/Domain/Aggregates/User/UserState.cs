using PAQK.Domain.Aggregates.User.Events;

namespace PAQK.Domain.Aggregates.User
{
    public class UserState
    {
        public string Id { get; private set; }
        public bool UserWasDeleted { get; set; }

        public void On(UserCreated e)
        {
            Id = e.Id;
        }

        public void On(UserDeleted e)
        {
            UserWasDeleted = true;
        }
    }
}