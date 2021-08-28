using System.Net;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Utilities.Configuration;

namespace WarOfEmpires.Utilities.Mail {
    [TransientServiceImplementation(typeof(IMailTemplate<ConfirmEmailMailTemplateParameters>))]
    public sealed class ConfirmEmailMailTemplate : IMailTemplate<ConfirmEmailMailTemplateParameters> {
        private readonly AppSettings _appSettings;

        public ConfirmEmailMailTemplate(AppSettings appSettings) {
            _appSettings = appSettings;
        }

        public MailMessage GetMessage(ConfirmEmailMailTemplateParameters parameters, string to) {
            return new MailMessage() {
                To = to,
                Subject = "Please confirm your email address",
                Body = $"<p>Please <a href=\"{_appSettings.ApplicationBaseUrl?.TrimEnd('/')}/Home/ConfirmEmail/?confirmationCode={parameters.ActivationCode}&email={WebUtility.UrlEncode(parameters.Email)}\">click here to confirm your new email address</a>.</p>"
            };
        }
    }
}