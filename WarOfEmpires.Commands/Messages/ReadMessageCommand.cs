namespace WarOfEmpires.Commands.Messages {
    public sealed class ReadMessageCommand : ICommand {
        public string Email { get; }
        public string MessageId { get; }

        public ReadMessageCommand(string email, string messageId) {
            Email = email;
            MessageId = messageId;
        }
    }
}