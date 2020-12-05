namespace WarOfEmpires.Commands.Alliances {
    public sealed class DisbandAllianceCommand : ICommand {
        public string Email { get; }

        public DisbandAllianceCommand(string email) {
            Email = email;
        }
    }
}
