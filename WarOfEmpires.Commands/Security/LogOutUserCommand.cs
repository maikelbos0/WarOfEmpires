namespace WarOfEmpires.Commands.Security {
    public sealed class LogOutUserCommand : ICommand {
        public string Email { get; }

        public LogOutUserCommand(string email) {
            Email = email;
        }
    }
}