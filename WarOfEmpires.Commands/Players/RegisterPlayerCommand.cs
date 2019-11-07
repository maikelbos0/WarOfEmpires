namespace WarOfEmpires.Commands.Players {
    public sealed class RegisterPlayerCommand : ICommand {
        public string Email { get; }
        public string DisplayName { get; }

        public RegisterPlayerCommand(string email, string displayName) {
            Email = email;
            DisplayName = displayName;
        }
    }
}