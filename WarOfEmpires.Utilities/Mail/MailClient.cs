using SendGrid;
using SendGrid.Helpers.Mail;
using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.Utilities.Configuration;

namespace WarOfEmpires.Utilities.Mail {
    [TransientServiceImplementation(typeof(IMailClient))]
    public sealed class MailClient : IMailClient {
        private readonly AppSettings _appSettings;

        public MailClient(AppSettings appSettings) {
            _appSettings = appSettings;
        }

        public void Send(MailMessage model) {
            var client = new SendGridClient(_appSettings.SendGridApiKey);
            var message = new SendGridMessage() {
                From = new EmailAddress(_appSettings.EmailFromAddress),
                Subject = model.Subject,
                HtmlContent = model.Body
            };

            message.AddTo(new EmailAddress(model.To));
            client.SendEmailAsync(message);
        }
    }
}