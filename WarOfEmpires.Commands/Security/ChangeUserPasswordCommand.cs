namespace WarOfEmpires.Commands.Security {
    public sealed class ChangeUserPasswordCommand : ICommand {
        public string Email { get; }
        public string CurrentPassword { get; }
        public string NewPassword { get; }

        public ChangeUserPasswordCommand(string email, string currentPassword, string newPassword) {
            Email = email;
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }
    }
}