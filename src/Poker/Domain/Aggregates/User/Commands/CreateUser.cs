using System;
using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.User.Commands
{
    public class CreateUser : Command
    {
        public new string Id
        {
            get { throw new InvalidOperationException(); }
            set { throw new InvalidOperationException(); }
        }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FacebookId { get; set; }
    }
}