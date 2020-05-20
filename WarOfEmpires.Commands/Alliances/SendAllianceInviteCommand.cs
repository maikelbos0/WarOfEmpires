namespace WarOfEmpires.Commands.Alliances {
    public sealed class SendAllianceInviteCommand : ICommand {
        public string Email { get; }
        public string PlayerId { get; }
        public string Message { get; }

        public SendAllianceInviteCommand(string email, string playerId, string message) {
            Email = email;
            PlayerId = playerId;
            Message = message;
        }
    }
}