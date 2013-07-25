using PAQK.Domain.Aggregates.Site.Events;

namespace PAQK.Domain.Aggregates.Site
{
    public class SiteState
    {
        public string Id { get; set; }

        public void On(SiteCreated e)
        {
            Id = e.Id;
        }
    }
}