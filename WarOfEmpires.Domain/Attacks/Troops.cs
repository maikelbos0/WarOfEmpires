using System;

namespace WarOfEmpires.Domain.Attacks {
    public sealed class Troops : Entity {
        public TroopType Type { get; private set; }
        public int Soldiers { get; private set; }
        public int Mercenaries { get; private set; }

        private Troops() { }

        public Troops(int soldiers, int mercenaries) {
            if (soldiers < 0) throw new ArgumentOutOfRangeException("soldiers", "Negative values are not allowed");
            if (mercenaries < 0) throw new ArgumentOutOfRangeException("mercenaries", "Negative values are not allowed");

            Soldiers = soldiers;
            Mercenaries = mercenaries;
        }

        public Troops(TroopType type, int soldiers, int mercenaries) : this(soldiers, mercenaries) {
            Type = type;
        }

        public int GetTotals() {
            return Soldiers + Mercenaries;
        }

        public Casualties ProcessCasualties(int casualties) {
            var mercenaryCasualties = Math.Min(Mercenaries, casualties);
            var soldierCasualties = Math.Min(Soldiers, casualties - mercenaryCasualties);

            Mercenaries -= mercenaryCasualties;
            Soldiers -= soldierCasualties;

            return new Casualties(Type, soldierCasualties, mercenaryCasualties);
        }

        public void Train(int soldiers, int mercenaries) {
            Mercenaries += mercenaries;
            Soldiers += soldiers;
        }

        public void Untrain(int soldiers, int mercenaries) {
            Mercenaries -= mercenaries;
            Soldiers -= soldiers;
        }
    }
}