namespace WarOfEmpires.Commands.Empires {
    public sealed class SetTaxCommand : ICommand {
        public string Email { get; }
        public string Tax { get; }

        public SetTaxCommand(string email, string tax) {
            Email = email;
            Tax = tax;
        }
    }
}