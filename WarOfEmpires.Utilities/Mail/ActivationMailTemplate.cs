using System.Net;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Utilities.Configuration;

namespace WarOfEmpires.Utilities.Mail {
    [TransientServiceImplementation(typeof(IMailTemplate<ActivationMailTemplateParameters>))]
    public sealed class ActivationMailTemplate : IMailTemplate<ActivationMailTemplateParameters> {
        private readonly AppSettings _appSettings;

        public ActivationMailTemplate(AppSettings appSettings) {
            _appSettings = appSettings;
        }

        public MailMessage GetMessage(ActivationMailTemplateParameters parameters, string to) {
            return new MailMessage() {
                To = to,
                Subject = "Please activate your account",
                Body = $"<p>Please <a href=\"{_appSettings.ApplicationBaseUrl?.TrimEnd('/')}/Home/Activate/?activationCode={parameters.ActivationCode}&email={WebUtility.UrlEncode(parameters.Email)}\">click here to activate your account</a>.</p>"
            };
        }
    }
}