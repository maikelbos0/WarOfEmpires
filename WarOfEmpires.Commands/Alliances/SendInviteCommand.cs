namespace WarOfEmpires.Commands.Alliances {
    public sealed class SendInviteCommand : ICommand {
        public string Email { get; }
        public int PlayerId { get; }
        public string Subject { get; }
        public string Body { get; }

        public SendInviteCommand(string email, int playerId, string subject, string body) {
            Email = email;
            PlayerId = playerId;
            Subject = subject;
            Body = body;
        }
    }
}