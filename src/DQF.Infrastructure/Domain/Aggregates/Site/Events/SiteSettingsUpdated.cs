using PAQK.Domain.Aggregates.Site.Data;
using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Site.Events
{
    public class SiteSettingsUpdated : Event
    {
        public SmtpSettingsData SmtpSettings { get; set; }
    }
}