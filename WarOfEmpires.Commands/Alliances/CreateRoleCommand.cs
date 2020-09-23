namespace WarOfEmpires.Commands.Alliances {
    public sealed class CreateRoleCommand : ICommand {
        public string Email { get; }
        public string Name { get; }
        public bool CanInvite { get; }

        public CreateRoleCommand(string email, string name, bool canInvite) {
            Email = email;
            Name = name;
            CanInvite = canInvite;
        }
    }
}