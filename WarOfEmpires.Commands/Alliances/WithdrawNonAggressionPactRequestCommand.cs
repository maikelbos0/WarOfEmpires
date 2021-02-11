namespace WarOfEmpires.Commands.Alliances {
    public sealed class WithdrawNonAggressionPactRequestCommand : ICommand {
        public string Email { get; }
        public int NonAggressionPactRequestId { get; }

        public WithdrawNonAggressionPactRequestCommand(string email, int nonAggressionPactRequestId) {
            Email = email;
            NonAggressionPactRequestId = nonAggressionPactRequestId;
        }
    }
}
