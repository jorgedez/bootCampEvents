using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Configuration;

namespace bootCamp.Shared.Helpers
{
    public static class EmailHelper
    {
        public static void SendMail(string subject, string body, string[] listTo)
        {
            var client = new SendGridClient(ConfigurationManager.AppSettings["SendGridKey"]);
            var msg = CreateMailMessage(subject, body, listTo, false);
            client.SendEmailAsync(msg).Wait();
        }

        private static SendGridMessage CreateMailMessage(string subject, string body, string[] listTo, bool isHtmlContent)
        {
            var from = new EmailAddress(ConfigurationManager.AppSettings["CorreoSend"], ConfigurationManager.AppSettings["CorreoSend"]);
            var tos = new List<EmailAddress>();

            foreach (var to in listTo)
            {
                tos.Add(new EmailAddress(to));
            }

            return MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, isHtmlContent ? null : body, isHtmlContent ? body : null);
        }
    }
}
