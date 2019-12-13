namespace WarOfEmpires.Commands.Attacks {
    public sealed class AttackCommand : ICommand {
        public string AttackerEmail { get; }
        public string DefenderId { get; }
        public string Turns { get; }

        public AttackCommand(string attackerEmail, string defenderId, string turns) {
            AttackerEmail = attackerEmail;
            DefenderId = defenderId;
            Turns = turns;
        }
    }
}