using System.Text.Json.Serialization;

namespace WarOfEmpires.Commands.Security {
    public sealed class ResetUserPasswordCommand : ICommand {
        public string Email { get; }
        [JsonIgnore]
        public string PasswordResetToken { get; }
        [JsonIgnore]
        public string NewPassword { get; }

        public ResetUserPasswordCommand(string email, string passwordResetToken, string newPassword) {
            Email = email;
            PasswordResetToken = passwordResetToken;
            NewPassword = newPassword;
        }
    }
}