namespace WarOfEmpires.Commands.Alliances {
    public sealed class SetRoleCommand : ICommand {
        public string Email { get; }
        public int PlayerId { get; }
        public int RoleId { get; }

        public SetRoleCommand(string email, int playerId, int roleId) {
            Email = email;
            PlayerId = playerId;
            RoleId = roleId;
        }
    }
}