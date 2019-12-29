namespace WarOfEmpires.Commands.Empires {
    public sealed class BankCommand : ICommand {
        public string Email { get; }

        public BankCommand(string email) {
            Email = email;
        }
    }
}