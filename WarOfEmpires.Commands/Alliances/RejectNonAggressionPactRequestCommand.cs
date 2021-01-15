namespace WarOfEmpires.Commands.Alliances {
    public sealed class RejectNonAggressionPactRequestCommand : ICommand {
        public string Email { get; }
        public int NonAggressionPactRequestId { get; }

        public RejectNonAggressionPactRequestCommand(string email, int nonAggressionPactRequestId) {
            Email = email;
            NonAggressionPactRequestId = nonAggressionPactRequestId;
        }
    }
}
