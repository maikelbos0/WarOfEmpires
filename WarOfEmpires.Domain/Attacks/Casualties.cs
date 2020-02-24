namespace WarOfEmpires.Domain.Attacks {
    public sealed class Casualties : Entity {
        public TroopType TroopType { get; private set; }
        public int Soldiers { get; private set; }
        public int Mercenaries { get; private set; }

        private Casualties() { }

        public Casualties(TroopType troopType, int soldiers, int mercenaries) {
            TroopType = troopType;
            Soldiers = soldiers;
            Mercenaries = mercenaries;
        }
    }
}