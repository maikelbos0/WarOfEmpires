namespace WarOfEmpires.Commands.Alliances {
    public sealed class SendInviteCommand : ICommand {
        public string Email { get; }
        public string PlayerId { get; }
        public string Subject { get; }
        public string Body { get; }

        public SendInviteCommand(string email, string playerId, string subject, string body) {
            Email = email;
            PlayerId = playerId;
            Subject = subject;
            Body = body;
        }
    }
}