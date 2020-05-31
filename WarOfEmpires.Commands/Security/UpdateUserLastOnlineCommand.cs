namespace WarOfEmpires.Commands.Security {
    public sealed class UpdateUserLastOnlineCommand : ICommand {
        public string Email { get; }

        public UpdateUserLastOnlineCommand(string email) {
            Email = email;
        }
    }
}