namespace WarOfEmpires.Commands.Alliances {
    public sealed class CreateAllianceCommand : ICommand {
        public string Email { get; }
        public string Code { get; }
        public string Name { get; }

        public CreateAllianceCommand(string email, string code, string name) {
            Email = email;
            Code = code;
            Name = name;
        }
    }
}