namespace WarOfEmpires.Commands.Alliances {
    public sealed class RemoveChatMessageCommand : ICommand {
        public string Email { get; }
        public string ChatMessageId { get; }

        public RemoveChatMessageCommand(string email, string chatMessageId) {
            Email = email;
            ChatMessageId = chatMessageId;
        }
    }
}