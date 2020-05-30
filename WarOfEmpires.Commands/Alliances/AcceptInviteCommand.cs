namespace WarOfEmpires.Commands.Alliances {
    public sealed class AcceptInviteCommand : ICommand {
        public string Email { get; }
        public string InviteId { get; }

        public AcceptInviteCommand(string email, string inviteId) {
            Email = email;
            InviteId = inviteId;
        }
    }
}