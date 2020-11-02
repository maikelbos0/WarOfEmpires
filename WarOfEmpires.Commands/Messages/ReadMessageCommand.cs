namespace WarOfEmpires.Commands.Messages {
    public sealed class ReadMessageCommand : ICommand {
        public string Email { get; }
        public int MessageId { get; }

        public ReadMessageCommand(string email, int messageId) {
            Email = email;
            MessageId = messageId;
        }
    }
}