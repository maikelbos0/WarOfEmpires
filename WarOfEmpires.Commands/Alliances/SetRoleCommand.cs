namespace WarOfEmpires.Commands.Alliances {
    public sealed class SetRoleCommand : ICommand {
        public string Email { get; }
        public string PlayerId { get; }
        public string RoleId { get; }

        public SetRoleCommand(string email, string playerId, string roleId) {
            Email = email;
            PlayerId = playerId;
            RoleId = roleId;
        }
    }
}