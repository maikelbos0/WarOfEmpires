namespace WarOfEmpires.Commands.Attacks {
    public sealed class AttackCommand : ICommand {
        public string AttackType { get; }
        public string AttackerEmail { get; }
        public int DefenderId { get; }
        public int Turns { get; }

        public AttackCommand(string attackType, string attackerEmail, int defenderId, int turns) {
            AttackType = attackType;
            AttackerEmail = attackerEmail;
            DefenderId = defenderId;
            Turns = turns;
        }
    }
}