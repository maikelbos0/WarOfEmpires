namespace WarOfEmpires.Commands.Players {
    public sealed class BlockPlayerCommand : ICommand {
        public string Email { get; }
        public int PlayerId { get; }

        public BlockPlayerCommand(string email, int playerId) {
            Email = email;
            PlayerId = playerId;
        }
    }
}
