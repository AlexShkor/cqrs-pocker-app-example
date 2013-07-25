using System;
using System.Net;
using System.Net.Mail;
using SendGridMail;
using SendGridMail.Transport;

namespace AKQ.Domain.Infrastructure.Services
{
    public class MailService
    {
        public void SendPasswordReseted(string email, string newPassword)
        {
            Send((message)=>
            {
                message.AddTo(email);
                message.Subject = "AKQbridge password reset";
                message.Text = string.Format("Your new password is {0}", newPassword);
            });
        }

        private void Send(Action<SendGrid> with)
        {
            var myMessage = SendGrid.GetInstance();
            myMessage.From = new MailAddress("akqbridge@hotmail.com");
            with(myMessage);
            var credentials = new NetworkCredential("e39c19f5-5d3a-4f8f-a1a3-4198d917823c@apphb.com", "u70inhtw");
            var smtp = SMTP.GetInstance(credentials);
            smtp.Deliver(myMessage);
        }
    }
}