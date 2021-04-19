using SendGrid;
using SendGrid.Helpers.Mail;
using System.Configuration;
using VDT.Core.DependencyInjection;

namespace WarOfEmpires.Utilities.Mail {
    [ScopedServiceImplementation(typeof(IMailClient))]
    public sealed class MailClient : IMailClient {
        public void Send(MailMessage model) {
            // TODO inject settings
            var client = new SendGridClient(ConfigurationManager.AppSettings["SendGrid.ApiKey"]);
            var message = new SendGridMessage() {
                From = new EmailAddress("maikel.bos0@gmail.com"),
                Subject = model.Subject,
                HtmlContent = model.Body
            };

            message.AddTo(new EmailAddress(model.To));
            client.SendEmailAsync(message);
        }
    }
}