namespace WarOfEmpires.Commands.Alliances {
    public sealed class LeaveAllianceCommand : ICommand {
        public string Email { get; }

        public LeaveAllianceCommand(string email) {
            Email = email;
        }
    }
}