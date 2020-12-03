namespace WarOfEmpires.Commands.Alliances {
    public sealed class TransferLeadershipCommand : ICommand {
        public string Email { get; }
        public int MemberId { get; }

        public TransferLeadershipCommand(string email, int memberId) {
            Email = email;
            MemberId = memberId;
        }
    }
}
