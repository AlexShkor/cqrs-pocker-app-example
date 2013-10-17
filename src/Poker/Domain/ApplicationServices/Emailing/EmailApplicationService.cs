using Poker.Domain.ApplicationServices.Emailing.Commands;
using Poker.Platform.Dispatching.Interfaces;
using Poker.Platform.Utilities;

namespace Poker.Domain.ApplicationServices.Emailing
{
    public class EmailApplicationService: IMessageHandler
    {
        private readonly SendGridUtil _sendGrid;

        public EmailApplicationService(SendGridUtil sendGrid)
        {
            _sendGrid = sendGrid;
        }

        public void Handle(SendMail c)
        {
            _sendGrid.SendMessage(c.Recipients, c.Subject, c.Body);
        }
    }
}