namespace WarOfEmpires.Commands.Markets {
    public sealed class WithdrawCaravanCommand : ICommand {
        public string Email { get; }
        public string CaravanId { get; }

        public WithdrawCaravanCommand(string email, string caravanId) {
            Email = email;
            CaravanId = caravanId;
        }
    }
}