namespace WarOfEmpires.Commands.Alliances {
    public sealed class RejectInviteCommand : ICommand {
        public string Email { get; }
        public int InviteId { get; }

        public RejectInviteCommand(string email, int inviteId) {
            Email = email;
            InviteId = inviteId;
        }
    }
}