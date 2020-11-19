namespace WarOfEmpires.Commands.Alliances {
    public sealed class DeleteChatMessageCommand : ICommand {
        public string Email { get; }
        public int ChatMessageId { get; }

        public DeleteChatMessageCommand(string email, int chatMessageId) {
            Email = email;
            ChatMessageId = chatMessageId;
        }
    }
}