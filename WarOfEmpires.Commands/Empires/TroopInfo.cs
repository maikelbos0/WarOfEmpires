namespace WarOfEmpires.Commands.Empires {
    public sealed class TroopInfo {
        public string Type { get; }
        public int? Soldiers { get; }
        public int? Mercenaries { get; }

        public TroopInfo(string type, int? soldiers, int? mercenaries) {
            Type = type;
            Soldiers = soldiers;
            Mercenaries = mercenaries;
        }
    }
}