namespace WarOfEmpires.Commands.Alliances {
    public sealed class WithdrawInviteCommand : ICommand {
        public string Email { get; }
        public string InviteId { get; }

        public WithdrawInviteCommand(string email, string inviteId) {
            Email = email;
            InviteId = inviteId;
        }
    }
}