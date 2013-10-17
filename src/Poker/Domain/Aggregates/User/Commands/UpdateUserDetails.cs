using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.User.Commands
{
    public class UpdateUserDetails: Command
    {
        public string UserName { get; set; }
        public string Id { get; set; }
    }
}