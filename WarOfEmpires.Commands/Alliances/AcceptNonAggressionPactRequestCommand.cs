namespace WarOfEmpires.Commands.Alliances {
    public sealed class AcceptNonAggressionPactRequestCommand : ICommand {
        public string Email { get; }
        public int NonAggressionPactRequestId { get; }

        public AcceptNonAggressionPactRequestCommand(string email, int nonAggressionPactRequestId) {
            Email = email;
            NonAggressionPactRequestId = nonAggressionPactRequestId;
        }
    }
}
