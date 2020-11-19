namespace WarOfEmpires.Commands.Alliances {
    public sealed class AcceptInviteCommand : ICommand {
        public string Email { get; }
        public int InviteId { get; }

        public AcceptInviteCommand(string email, int inviteId) {
            Email = email;
            InviteId = inviteId;
        }
    }
}