using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;

namespace WarOfEmpires.Domain.Players {
    public sealed class Casualties : ValueObject {
        // TODO make this a single troop type casualty Entity
        public Troops Archers { get; private set; }
        public Troops Cavalry { get; private set; }
        public Troops Footmen { get; private set; }

        private Casualties() { }

        public Casualties(Troops archers, Troops cavalry, Troops footmen) {
            Archers = archers;
            Cavalry = cavalry;
            Footmen = footmen;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            // TODO fix equality
            yield return Archers.Soldiers;
            yield return Archers.Mercenaries;
            yield return Cavalry.Soldiers;
            yield return Cavalry.Mercenaries;
            yield return Footmen.Soldiers;
            yield return Footmen.Mercenaries;
        }
    }
}