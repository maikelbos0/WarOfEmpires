namespace WarOfEmpires.Commands.Empires {
    public sealed class DepositCommand : ICommand {
        public string Email { get; }

        public DepositCommand(string email) {
            Email = email;
        }
    }
}