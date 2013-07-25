using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.User.Commands
{
    public class DeleteUser: Command
    {
        public string DeletedByUserId { get; set; }
    }
}