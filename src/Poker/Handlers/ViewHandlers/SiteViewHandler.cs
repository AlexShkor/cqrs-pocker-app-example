using Poker.Databases;
using Poker.Domain.Aggregates.Site.Events;
using Poker.Platform.Dispatching;
using Poker.Platform.Dispatching.Attributes;
using Poker.Platform.Dispatching.Interfaces;
using Poker.Views;
using Uniform;

namespace Poker.Handlers.ViewHandlers
{
    [Priority(PriorityStages.ViewHandling)]
    public class SiteViewHandler : IMessageHandler
    {
        private readonly IDocumentCollection<SiteView> _sites;

        public SiteViewHandler(ViewDatabase db)
        {
            _sites = db.Sites;
        }

        public void Handle(SiteCreated e)
        {
            _sites.Save(e.Id, site => { });
        }

        public void Handle(SiteSettingsUpdated e)
        {
            _sites.Update(e.Id, site => site.SmtpSettings = e.SmtpSettings);
        }

        public void Handle(SchedulerStarted e)
        {
            _sites.Update(e.Id, site => site.SchedulerStopped = false);
        }

        public void Handle(SchedulerStopped e)
        {
            _sites.Update(e.Id, site =>
            {
                site.SchedulerStopped = true;
                site.SchedulerRestartNeeded = e.Restart;
            });
        }
    }
}