using System;

namespace WarOfEmpires.Domain.Attacks {
    public class Troops : Entity {
        public virtual TroopType Type { get; private set; }
        public virtual int Soldiers { get; private set; }
        public virtual int Mercenaries { get; private set; }

        protected Troops() { }

        public Troops(TroopType type, int soldiers, int mercenaries) {
            Type = type;
            Soldiers = soldiers;
            Mercenaries = mercenaries;
        }

        public int GetTotals() {
            return Soldiers + Mercenaries;
        }

        public Casualties ProcessCasualties(int casualties) {
            var mercenaryCasualties = Math.Min(Mercenaries, casualties);
            var soldierCasualties = Math.Min(Soldiers, casualties - mercenaryCasualties);

            Soldiers -= soldierCasualties;
            Mercenaries -= mercenaryCasualties;

            return new Casualties(Type, soldierCasualties, mercenaryCasualties);
        }

        public void Train(int soldiers, int mercenaries) {
            Soldiers += soldiers;
            Mercenaries += mercenaries;
        }

        public void Untrain(int soldiers, int mercenaries) {
            Soldiers -= soldiers;
            Mercenaries -= mercenaries;
        }
    }
}