namespace WarOfEmpires.Commands.Alliances {
    public sealed class ClearRoleCommand : ICommand {
        public string Email { get; }
        public string PlayerId { get; }

        public ClearRoleCommand(string email, string playerId) {
            Email = email;
            PlayerId = playerId;
        }
    }
}