using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Site.Commands
{
    public class CreateSite : Command
    {
        public string Id { get; set; }
    }
}