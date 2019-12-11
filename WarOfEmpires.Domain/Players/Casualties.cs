using System.Collections.Generic;

namespace WarOfEmpires.Domain.Players {
    public sealed class Casualties : ValueObject {
        public Troops Archers { get; }
        public Troops Cavalry { get; }
        public Troops Footmen { get; }

        public Casualties(Troops archers, Troops cavalry, Troops footmen) {
            Archers = archers;
            Cavalry = cavalry;
            Footmen = footmen;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return Archers.Soldiers;
            yield return Archers.Mercenaries;
            yield return Cavalry.Soldiers;
            yield return Cavalry.Mercenaries;
            yield return Footmen.Soldiers;
            yield return Footmen.Mercenaries;
        }
    }
}