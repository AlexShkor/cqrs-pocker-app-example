using PAQK.Domain.Aggregates.Site.Events;
using PAQK.Platform.Domain;

namespace PAQK.Domain.Aggregates.Site
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