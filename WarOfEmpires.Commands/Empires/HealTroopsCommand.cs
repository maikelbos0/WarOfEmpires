namespace WarOfEmpires.Commands.Empires {
    public sealed class HealTroopsCommand : ICommand {
        public string Email { get; }
        public string CurrentStamina { get; }
    }
}