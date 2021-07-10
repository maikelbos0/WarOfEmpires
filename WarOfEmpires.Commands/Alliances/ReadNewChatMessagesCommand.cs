namespace WarOfEmpires.Commands.Alliances {
    public sealed class ReadNewChatMessagesCommand : ICommand {
        public string Email { get; }

        public ReadNewChatMessagesCommand(string email) {
            Email = email;
        }
    }
}
