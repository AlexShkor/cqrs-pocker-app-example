using Poker.Domain.Aggregates.Site.Events;
using Poker.Platform.Domain;

namespace Poker.Domain.Aggregates.Site
{
    public sealed class SiteState : AggregateState
    {
        public string Id { get; set; }

        public SiteState()
        {
            On((SiteCreated e) => Id = e.Id);
        }
    }
}