namespace WarOfEmpires.Commands.Alliances {
    public sealed class CreateRoleCommand : ICommand {
        public string Email { get; }
        public string Name { get; }

        public CreateRoleCommand(string email, string name) {
            Email = email;
            Name = name;
        }
    }
}