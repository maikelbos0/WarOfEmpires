namespace WarOfEmpires.Commands.Attacks {
    public sealed class ReadAttackCommand : ICommand {
        public string Email { get; }
        public int AttackId { get; }

        public ReadAttackCommand(string email, int attackId) {
            Email = email;
            AttackId = attackId;
        }
    }
}