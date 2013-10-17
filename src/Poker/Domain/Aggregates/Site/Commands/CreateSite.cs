using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Site.Commands
{
    public class CreateSite : Command
    {
        public string Id { get; set; }
    }
}