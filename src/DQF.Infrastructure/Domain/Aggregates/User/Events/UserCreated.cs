using System;
using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.User.Events
{
    public class UserCreated : Event
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreationDate { get; set; }
    }
}