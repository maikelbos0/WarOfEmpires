namespace WarOfEmpires.Commands.Alliances {
    public sealed class ReadInviteCommand : ICommand {
        public string Email { get; }
        public int InviteId { get; }

        public ReadInviteCommand(string email, int inviteId) {
            Email = email;
            InviteId = inviteId;
        }
    }
}