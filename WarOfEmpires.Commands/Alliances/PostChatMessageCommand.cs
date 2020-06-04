namespace WarOfEmpires.Commands.Alliances {
    public sealed class PostChatMessageCommand : ICommand {
        public string Email { get; }
        public string Message { get; }

        public PostChatMessageCommand(string email, string message) {
            Email = email;
            Message = message;
        }
    }
}