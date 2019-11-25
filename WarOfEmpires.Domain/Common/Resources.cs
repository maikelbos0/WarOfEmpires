using System;
using System.Collections.Generic;

namespace WarOfEmpires.Domain.Common {
    public sealed class Resources : ValueObject {
        public int Gold { get; private set; }
        public int Food { get; private set; }
        public int Wood { get; private set; }
        public int Stone { get; private set; }
        public int Ore { get; private set; }

        public Resources(int gold, int food, int wood, int stone, int ore) {
            if (gold < 0) throw new ArgumentOutOfRangeException("gold", "Negative values are not allowed");
            if (food < 0) throw new ArgumentOutOfRangeException("food", "Negative values are not allowed");
            if (wood < 0) throw new ArgumentOutOfRangeException("wood", "Negative values are not allowed");
            if (stone < 0) throw new ArgumentOutOfRangeException("stone", "Negative values are not allowed");
            if (ore < 0) throw new ArgumentOutOfRangeException("ore", "Negative values are not allowed");

            Gold = gold;
            Food = food;
            Wood = wood;
            Stone = stone;
            Ore = ore;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return Gold;
            yield return Food;
            yield return Wood;
            yield return Stone;
            yield return Ore;
        }

        public bool CanAfford(Resources r) {
            return Gold >= r.Gold
                && Food >= r.Food
                && Wood >= r.Wood
                && Stone >= r.Stone
                && Ore >= r.Ore;
        }

        public static Resources operator +(Resources a, Resources b) {
            return new Resources(a.Gold + b.Gold, a.Food + b.Food, a.Wood + b.Wood, a.Stone + b.Stone, a.Ore + b.Ore);
        }

        public static Resources operator -(Resources a, Resources b) {
            return new Resources(a.Gold - b.Gold, a.Food - b.Food, a.Wood - b.Wood, a.Stone - b.Stone, a.Ore - b.Ore);
        }

        public static Resources operator *(Resources a, int b) {
            return new Resources(a.Gold * b, a.Food * b, a.Wood * b, a.Stone * b, a.Ore * b);
        }

        public static Resources operator *(int a, Resources b) {
            return b * a;
        }
    }
}