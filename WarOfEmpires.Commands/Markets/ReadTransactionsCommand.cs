namespace WarOfEmpires.Commands.Markets {
    public sealed class ReadTransactionsCommand : ICommand {
        public string Email { get; }

        public ReadTransactionsCommand(string email) {
            Email = email;
        }
    }
}