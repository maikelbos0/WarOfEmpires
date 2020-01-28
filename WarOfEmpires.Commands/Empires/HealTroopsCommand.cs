namespace WarOfEmpires.Commands.Empires {
    public sealed class HealTroopsCommand : ICommand {
        public string Email { get; }
        public string StaminaToHeal { get; }

        public HealTroopsCommand(string email, string staminaToHeal) {
            Email = email;
            StaminaToHeal = staminaToHeal;
        }
    }
}