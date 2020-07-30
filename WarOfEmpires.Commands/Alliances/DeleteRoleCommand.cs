namespace WarOfEmpires.Commands.Alliances {
    public sealed class DeleteRoleCommand : ICommand {
        public string Email { get; }
        public string RoleId { get; }

        public DeleteRoleCommand(string email, string roleId) {
            Email = email;
            RoleId = roleId;
        }
    }
}