using System;
using Poker.Domain.Aggregates.User.Events;
using Poker.Platform.Domain;

namespace Poker.Domain.Aggregates.User
{
    public sealed class UserState: AggregateState
    {
        public string Id { get; private set; }
        public bool UserWasDeleted { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreationDate { get; set; }

        public UserState()
        {
            On((UserCreated e) => Id = e.Id);
            On((UserDeleted e) => UserWasDeleted = true);
        }
    }
}