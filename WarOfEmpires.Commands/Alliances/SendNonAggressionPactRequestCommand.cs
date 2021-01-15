namespace WarOfEmpires.Commands.Alliances {
    public sealed class SendNonAggressionPactRequestCommand : ICommand {
        public string Email { get; }
        public int AllianceId { get; }

        public SendNonAggressionPactRequestCommand(string email, int allianceId) {
            Email = email;
            AllianceId = allianceId;
        }
    }
}
