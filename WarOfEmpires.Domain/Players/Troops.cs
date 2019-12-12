using System;
using System.Collections.Generic;

namespace WarOfEmpires.Domain.Players {
    public sealed class Troops : ValueObject {
        public int Soldiers { get; private set; }
        public int Mercenaries { get; private set; }

        public Troops(int soldiers, int mercenaries) {
            if (soldiers < 0) throw new ArgumentOutOfRangeException("soldiers", "Negative values are not allowed");
            if (mercenaries < 0) throw new ArgumentOutOfRangeException("mercenaries", "Negative values are not allowed");

            Soldiers = soldiers;
            Mercenaries = mercenaries;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return Soldiers;
            yield return Mercenaries;
        }

        public int GetTotals() {
            return Soldiers + Mercenaries;
        }

        public Troops GetTroopCasualties(int casualties) {
            var mercenaryCasualties = Math.Min(Mercenaries, casualties);
            var soldierCasualties = Math.Min(Soldiers, casualties - mercenaryCasualties);

            return new Troops(soldierCasualties, mercenaryCasualties);
        }

        public static Troops operator +(Troops t1, Troops t2) {
            return new Troops(t1.Soldiers + t2.Soldiers, t1.Mercenaries + t2.Mercenaries);
        }

        public static Troops operator -(Troops t1, Troops t2) {
            return new Troops(t1.Soldiers - t2.Soldiers, t1.Mercenaries - t2.Mercenaries);
        }
    }
}