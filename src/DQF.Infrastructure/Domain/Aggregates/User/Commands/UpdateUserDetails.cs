using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.User.Commands
{
    public class UpdateUserDetails: Command
    {
        public string UserName { get; set; }
    }
}