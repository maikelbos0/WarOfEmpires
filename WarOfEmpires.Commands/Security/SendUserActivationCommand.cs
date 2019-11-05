namespace WarOfEmpires.Commands.Security {
    public sealed class SendUserActivationCommand : ICommand {
        public string Email { get; }

        public SendUserActivationCommand(string email) {
            Email = email;
        }
    }
}