using WarOfEmpires.Utilities.Configuration;
using WarOfEmpires.Utilities.Container;
using System.Net;

namespace WarOfEmpires.Utilities.Mail {
    [InterfaceInjectable]
    public sealed class ActivationMailTemplate : IMailTemplate<ActivationMailTemplateParameters> {
        private readonly IAppSettings _appSettings;

        public ActivationMailTemplate(IAppSettings appSettings) {
            _appSettings = appSettings;
        }

        public MailMessage GetMessage(ActivationMailTemplateParameters parameters, string to) {
            return new MailMessage() {
                To = to,
                Subject = "Please activate your account",
                Body = $"<p>Please <a href=\"{_appSettings["Application.BaseUrl"]}Home/Activate/?activationCode={parameters.ActivationCode}&email={WebUtility.UrlEncode(parameters.Email)}\">click here to activate your account</a>.</p>"
            };
        }
    }
}