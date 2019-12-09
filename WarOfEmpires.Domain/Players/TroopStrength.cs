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

        public static TroopStrength operator *(TroopStrength a, int b) {
            return new TroopStrength(a.Attack * b, a.Defense * b);
        }

        public static TroopStrength operator *(int a, TroopStrength b) {
            return b * a;
        }
    }
}