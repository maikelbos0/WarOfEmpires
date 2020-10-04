namespace WarOfEmpires.Commands.Alliances {
    public sealed class DeleteChatMessageCommand : ICommand {
        public string Email { get; }
        public string ChatMessageId { get; }

        public DeleteChatMessageCommand(string email, string chatMessageId) {
            Email = email;
            ChatMessageId = chatMessageId;
        }
    }
}