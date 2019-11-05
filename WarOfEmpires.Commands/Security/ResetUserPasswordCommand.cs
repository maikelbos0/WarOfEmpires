namespace WarOfEmpires.Commands.Security {
    public sealed class ResetUserPasswordCommand : ICommand {
        public string Email { get; }
        public string PasswordResetToken { get; }
        public string NewPassword { get; }

        public ResetUserPasswordCommand(string email, string passwordResetToken, string newPassword) {
            Email = email;
            PasswordResetToken = passwordResetToken;
            NewPassword = newPassword;
        }
    }
}