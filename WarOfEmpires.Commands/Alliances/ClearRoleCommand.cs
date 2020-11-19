namespace WarOfEmpires.Commands.Alliances {
    public sealed class ClearRoleCommand : ICommand {
        public string Email { get; }
        public int PlayerId { get; }

        public ClearRoleCommand(string email, int playerId) {
            Email = email;
            PlayerId = playerId;
        }
    }
}