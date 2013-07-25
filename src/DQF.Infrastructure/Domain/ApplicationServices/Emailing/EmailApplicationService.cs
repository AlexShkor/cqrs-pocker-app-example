using PAQK.Domain.ApplicationServices.Emailing.Commands;
using PAQK.Platform.Dispatching.Interfaces;
using PAQK.Platform.Utilities;

namespace PAQK.Domain.ApplicationServices.Emailing
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