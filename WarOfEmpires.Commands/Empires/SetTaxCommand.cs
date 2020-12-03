namespace WarOfEmpires.Commands.Empires {
    public sealed class SetTaxCommand : ICommand {
        public string Email { get; }
        public int Tax { get; }

        public SetTaxCommand(string email, int tax) {
            Email = email;
            Tax = tax;
        }
    }
}