namespace WarOfEmpires.Commands.Alliances {
    public sealed class DeleteRoleCommand : ICommand {
        public string Email { get; }
        public int RoleId { get; }

        public DeleteRoleCommand(string email, int roleId) {
            Email = email;
            RoleId = roleId;
        }
    }
}