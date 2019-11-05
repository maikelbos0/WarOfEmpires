using WarOfEmpires.Utilities.Configuration;
using WarOfEmpires.Utilities.Container;
using System.Net;

namespace WarOfEmpires.Utilities.Mail {
    [InterfaceInjectable]
    public sealed class ConfirmEmailMailTemplate : IMailTemplate<ConfirmEmailMailTemplateParameters> {
        private readonly IAppSettings _appSettings;

        public ConfirmEmailMailTemplate(IAppSettings appSettings) {
            _appSettings = appSettings;
        }

        public MailMessage GetMessage(ConfirmEmailMailTemplateParameters parameters, string to) {
            return new MailMessage() {
                To = to,
                Subject = "Please confirm your email address",
                Body = $"<p>Please <a href=\"{_appSettings["Application.BaseUrl"]}Home/ConfirmEmail/?confirmationCode={parameters.ActivationCode}&email={WebUtility.UrlEncode(parameters.Email)}\">click here to confirm your new email address</a>.</p>"
            };
        }
    }
}