namespace WarOfEmpires.Commands.Alliances {
    public sealed class DepositCommand : ICommand {
        public string Email { get; }
        public int? Gold { get; }
        public int? Food { get; }
        public int? Wood { get; }
        public int? Stone { get; }
        public int? Ore { get; }

        public DepositCommand(string email, int? gold, int? food, int? wood, int? stone, int? ore) {
            Email = email;
            Gold = gold;
            Food = food;
            Wood = wood;
            Stone = stone;
            Ore = ore;
        }
    }
}
