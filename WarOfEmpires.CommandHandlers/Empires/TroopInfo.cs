using WarOfEmpires.Domain.Attacks;

namespace WarOfEmpires.CommandHandlers.Empires {
    internal class TroopInfo {
        public TroopType Type { get; }
        public int Soldiers { get; }
        public int Mercenaries { get; }

        public TroopInfo(TroopType type, int soldiers, int mercenaries) {
            Type = type;
            Soldiers = soldiers;
            Mercenaries = mercenaries;
        }
    }
}
