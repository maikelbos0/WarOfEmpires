namespace WarOfEmpires.Commands.Empires {
    public sealed class HealTroopsCommand : ICommand {
        public string Email { get; }
        public int StaminaToHeal { get; }

        public HealTroopsCommand(string email, int staminaToHeal) {
            Email = email;
            StaminaToHeal = staminaToHeal;
        }
    }
}