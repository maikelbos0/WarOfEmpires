namespace WarOfEmpires.Commands.Empires {
    public sealed class TrainTroopsCommand : ICommand {
        public string Email { get; }
        public string Archers { get; }
        public string MercenaryArchers { get; }
        public string Cavalry { get; }
        public string MercenaryCavalry { get; }
        public string Footmen { get; }
        public string MercenaryFootmen { get; }

        public TrainTroopsCommand(string email, string archers, string mercenaryArchers, string cavalry, string mercenaryCavalry, string footmen, string mercenaryFootmen) {
            Email = email;
            Archers = archers;
            MercenaryArchers = mercenaryArchers;
            Cavalry = cavalry;
            MercenaryCavalry = mercenaryCavalry;
            Footmen = footmen;
            MercenaryFootmen = mercenaryFootmen;
        }
    }
}
