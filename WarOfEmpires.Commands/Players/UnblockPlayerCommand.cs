namespace WarOfEmpires.Commands.Players {
    public sealed class UnblockPlayerCommand : ICommand {
        public string Email { get; }
        public int PlayerId { get; }

        public UnblockPlayerCommand(string email, int playerId) {
            Email = email;
            PlayerId = playerId;
        }
    }
}
