using System;
using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.User.Commands
{
    public class ChangePassword: Command
    {
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime Date { get; set; }
        public bool IsChangedByAdmin { get; set; }
    }
}