namespace WarOfEmpires.Commands.Security {
    public sealed class ForgotUserPasswordCommand : ICommand {
        public string Email { get; }

        public ForgotUserPasswordCommand(string email) {
            Email = email;
        }
    }
}