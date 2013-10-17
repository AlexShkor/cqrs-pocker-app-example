using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.User.Commands
{
    public class DeleteUser: Command
    {
        public string DeletedByUserId { get; set; }
    }
}