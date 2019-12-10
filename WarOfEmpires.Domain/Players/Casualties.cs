using System.Collections.Generic;

namespace WarOfEmpires.Domain.Players {
    public sealed class Casualties : ValueObject {
        public int Soldiers { get; }
        public int Mercenaries { get; }

        public Casualties(int soldiers, int mercenaries) {
            Soldiers = soldiers;
            Mercenaries = mercenaries;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return Soldiers;
            yield return Mercenaries;
        }
    }
}