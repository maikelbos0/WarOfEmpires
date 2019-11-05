namespace WarOfEmpires.Commands.Security {
    public sealed class ConfirmUserEmailChangeCommand : ICommand {
        public string Email { get; }
        public string ConfirmationCode { get; }

        public ConfirmUserEmailChangeCommand(string email, string confirmationCode) {
            Email = email;
            ConfirmationCode = confirmationCode;
        }
    }
}