namespace WarOfEmpires.Commands.Empires {
    public sealed class TroopInfo {
        public string Type { get; }
        public string Soldiers { get; }
        public string Mercenaries { get; }

        public TroopInfo(string type, string soldiers, string mercenaries) {
            Type = type;
            Soldiers = soldiers;
            Mercenaries = mercenaries;
        }
    }
}