namespace WarOfEmpires.Commands.Alliances {
    public sealed class WithdrawInviteCommand : ICommand {
        public string Email { get; }
        public int InviteId { get; }

        public WithdrawInviteCommand(string email, int inviteId) {
            Email = email;
            InviteId = inviteId;
        }
    }
}