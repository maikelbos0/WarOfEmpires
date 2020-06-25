namespace WarOfEmpires.Commands.Alliances {
    public sealed class ReadInviteCommand : ICommand {
        public string Email { get; }
        public string InviteId { get; }

        public ReadInviteCommand(string email, string inviteId) {
            Email = email;
            InviteId = inviteId;
        }
    }
}