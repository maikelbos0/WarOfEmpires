namespace WarOfEmpires.Commands.Alliances {
    public sealed class KickFromAllianceCommand : ICommand {
        public string Email { get; }
        public string PlayerID { get; }

        public KickFromAllianceCommand(string email, string playerId) {
            Email = email;
            PlayerID = playerId;
        }
    }
}