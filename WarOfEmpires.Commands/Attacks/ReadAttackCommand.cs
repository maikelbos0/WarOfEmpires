namespace WarOfEmpires.Commands.Attacks {
    public sealed class ReadAttackCommand : ICommand {
        public string Email { get; }
        public string AttackId { get; }

        public ReadAttackCommand(string email, string attackId) {
            Email = email;
            AttackId = attackId;
        }
    }
}