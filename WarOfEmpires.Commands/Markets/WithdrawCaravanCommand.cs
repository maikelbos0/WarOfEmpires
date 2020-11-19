namespace WarOfEmpires.Commands.Markets {
    public sealed class WithdrawCaravanCommand : ICommand {
        public string Email { get; }
        public int CaravanId { get; }

        public WithdrawCaravanCommand(string email, int caravanId) {
            Email = email;
            CaravanId = caravanId;
        }
    }
}