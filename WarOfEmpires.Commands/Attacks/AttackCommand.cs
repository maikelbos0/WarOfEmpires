namespace WarOfEmpires.Commands.Attacks {
    public sealed class AttackCommand : ICommand {
        public string AttackType { get; }
        public string AttackerEmail { get; }
        public string DefenderId { get; }
        public string Turns { get; }

        public AttackCommand(string attackType, string attackerEmail, string defenderId, string turns) {
            AttackType = attackType;
            AttackerEmail = attackerEmail;
            DefenderId = defenderId;
            Turns = turns;
        }
    }
}