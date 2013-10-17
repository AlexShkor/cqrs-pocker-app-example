using Poker.Domain.Aggregates.Site.Data;
using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Site.Events
{
    public class SiteSettingsUpdated : Event
    {
        public SmtpSettingsData SmtpSettings { get; set; }
    }
}