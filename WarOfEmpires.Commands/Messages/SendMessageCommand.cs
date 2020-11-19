namespace WarOfEmpires.Commands.Messages {
    public sealed class SendMessageCommand : ICommand {
        public string SenderEmail { get; }
        public int RecipientId { get; }
        public string Subject { get; }
        public string Body { get; }

        public SendMessageCommand(string senderEmail, int recipientId, string subject, string body) {
            SenderEmail = senderEmail;
            RecipientId = recipientId;
            Subject = subject;
            Body = body;
        }
    }
}