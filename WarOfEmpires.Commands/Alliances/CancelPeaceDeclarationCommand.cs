namespace WarOfEmpires.Commands.Alliances {
    public sealed class CancelPeaceDeclarationCommand : ICommand {
        public string Email { get; }
        public int WarId { get; }

        public CancelPeaceDeclarationCommand(string email, int warId) {
            Email = email;
            WarId = warId;
        }
    }
}
