using System;
using System.Collections.Generic;

namespace WarOfEmpires.Domain.Players {
    public sealed class TroopStrength : ValueObject {
        public int Attack { get; }
        public int Defense { get; }

        public TroopStrength(int attack, int defense) {
            if (attack < 0) throw new ArgumentOutOfRangeException("attack", "Negative values are not allowed");
            if (defense < 0) throw new ArgumentOutOfRangeException("defense", "Negative values are not allowed");

            Attack = attack;
            Defense = defense;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return Attack;
            yield return Defense;
        }
    }
}