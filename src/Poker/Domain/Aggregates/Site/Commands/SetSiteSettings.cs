using Poker.Domain.Aggregates.Site.Data;
using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Site.Commands
{
    public class SetSiteSettings : Command
    {
        public SmtpSettingsData SmtpSettings { get; set; }
    }
}