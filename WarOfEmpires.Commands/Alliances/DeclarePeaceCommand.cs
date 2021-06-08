namespace WarOfEmpires.Commands.Alliances {
    public sealed class DeclarePeaceCommand : ICommand {
        public string Email { get; }
        public int WarId { get; }

        public DeclarePeaceCommand(string email, int warId) {
            Email = email;
            WarId = warId;
        }
    }
}
