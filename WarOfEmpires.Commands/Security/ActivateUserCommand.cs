namespace WarOfEmpires.Commands.Security {
    public sealed class ActivateUserCommand : ICommand {
        public string Email { get; }
        public string ActivationCode { get; }

        public ActivateUserCommand(string email, string activationCode) {
            Email = email;
            ActivationCode = activationCode;
        }
    }
}