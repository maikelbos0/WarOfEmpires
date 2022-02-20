using System.Net;
using WarOfEmpires.Utilities.Configuration;

namespace WarOfEmpires.Utilities.Mail {
    public sealed class PasswordResetMailTemplate : IMailTemplate<PasswordResetMailTemplateParameters> {
        private readonly AppSettings _appSettings;

        public PasswordResetMailTemplate(AppSettings appSettings) {
            _appSettings = appSettings;
        }

        public MailMessage GetMessage(PasswordResetMailTemplateParameters parameters, string to) {
            return new MailMessage() {
                To = to,
                Subject = "Your password reset request",
                Body = $"<p>Please <a href=\"{_appSettings.ApplicationBaseUrl?.TrimEnd('/')}/Home/ResetPassword/?email={WebUtility.UrlEncode(parameters.Email)}&token={WebUtility.UrlEncode(parameters.Token)}\">click here to reset your password</a>.</p>"
            };
        }
    }
}