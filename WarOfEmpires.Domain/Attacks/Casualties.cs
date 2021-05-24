namespace WarOfEmpires.Domain.Attacks {
    public class Casualties : Entity {
        public virtual TroopType TroopType { get; private set; }
        public virtual int Soldiers { get; private set; }
        public virtual int Mercenaries { get; private set; }

        protected Casualties() { }

        public Casualties(TroopType troopType, int soldiers, int mercenaries) {
            TroopType = troopType;
            Soldiers = soldiers;
            Mercenaries = mercenaries;
        }
    }
}