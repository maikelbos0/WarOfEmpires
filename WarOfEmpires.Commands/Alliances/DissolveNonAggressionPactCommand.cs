namespace WarOfEmpires.Commands.Alliances {
    public sealed class DissolveNonAggressionPactCommand : ICommand {
        public string Email { get; }
        public int NonAggressionPactId { get; }

        public DissolveNonAggressionPactCommand(string email, int nonAggressionPactId) {
            Email = email;
            NonAggressionPactId = nonAggressionPactId;
        }
    }
}
