namespace WarOfEmpires.Commands.Alliances {
    public sealed class CreateRoleCommand : ICommand {
        public string Email { get; }
        public string Name { get; }
        public bool CanInvite { get; }
        public bool CanManageRoles { get; }

        public CreateRoleCommand(string email, string name, bool canInvite, bool canManageRoles) {
            Email = email;
            Name = name;
            CanInvite = canInvite;
            CanManageRoles = canManageRoles;
        }
    }
}