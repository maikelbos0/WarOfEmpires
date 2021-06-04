namespace WarOfEmpires.Commands.Alliances {
    public sealed class DeclareWarCommand : ICommand {
        public string Email { get; }
        public int AllianceId { get; }

        public DeclareWarCommand(string email, int allianceId) {
            Email = email;
            AllianceId = allianceId;
        }
    }
}
