using PAQK.Domain.Aggregates.Site.Data;
using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Site.Commands
{
    public class SetSiteSettings : Command
    {
        public SmtpSettingsData SmtpSettings { get; set; }
    }
}