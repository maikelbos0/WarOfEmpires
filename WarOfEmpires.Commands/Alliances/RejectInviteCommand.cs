namespace WarOfEmpires.Commands.Alliances {
    public sealed class RejectInviteCommand : ICommand {
        public string Email { get; }
        public string InviteId { get; }

        public RejectInviteCommand(string email, string inviteId) {
            Email = email;
            InviteId = inviteId;
        }
    }
}