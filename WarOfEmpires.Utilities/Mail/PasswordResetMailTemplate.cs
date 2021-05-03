using System.Net;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Utilities.Configuration;

namespace WarOfEmpires.Utilities.Mail {
    [ScopedServiceImplementation(typeof(IMailTemplate<PasswordResetMailTemplateParameters>))]
    public sealed class PasswordResetMailTemplate : IMailTemplate<PasswordResetMailTemplateParameters> {
        private readonly AppSettings _appSettings;

        public PasswordResetMailTemplate(AppSettings appSettings) {
            _appSettings = appSettings;
        }

        public MailMessage GetMessage(PasswordResetMailTemplateParameters parameters, string to) {
            return new MailMessage() {
                To = to,
                Subject = "Your password reset request",
                Body = $"<p>Please <a href=\"{_appSettings.ApplicationBaseUrl}Home/ResetPassword/?email={WebUtility.UrlEncode(parameters.Email)}&token={WebUtility.UrlEncode(parameters.Token)}\">click here to reset your password</a>.</p>"
            };
        }
    }
}