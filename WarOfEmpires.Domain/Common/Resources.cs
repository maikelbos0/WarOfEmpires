using System;
using System.Collections.Generic;

namespace WarOfEmpires.Domain.Common {
    public sealed class Resources : ValueObject {
        public long Gold { get; private set; }
        public long Food { get; private set; }
        public long Wood { get; private set; }
        public long Stone { get; private set; }
        public long Ore { get; private set; }

        private Resources() {
        }

        public Resources(long gold = 0, long food = 0, long wood = 0, long stone = 0, long ore = 0) {
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

        public Resources SubtractSafe(Resources value) {
            return SubtractSafe(value, out _);
        }

        public Resources SubtractSafe(Resources value, out Resources remainder) {
            var safeValue = new Resources(
                Math.Min(Gold, value.Gold),
                Math.Min(Food, value.Food),
                Math.Min(Wood, value.Wood),
                Math.Min(Stone, value.Stone),
                Math.Min(Ore, value.Ore)
            );

            remainder = value - safeValue;

            return this - safeValue;
        }

        public static Resources operator +(Resources a, Resources b) {
            return new Resources(a.Gold + b.Gold, a.Food + b.Food, a.Wood + b.Wood, a.Stone + b.Stone, a.Ore + b.Ore);
        }

        public static Resources operator -(Resources a, Resources b) {
            return new Resources(a.Gold - b.Gold, a.Food - b.Food, a.Wood - b.Wood, a.Stone - b.Stone, a.Ore - b.Ore);
        }

        public static Resources operator *(Resources a, long b) {
            return new Resources(a.Gold * b, a.Food * b, a.Wood * b, a.Stone * b, a.Ore * b);
        }

        public static Resources operator *(long a, Resources b) {
            return b * a;
        }

        public static Resources operator *(Resources a, decimal b) {
            return new Resources((long)(a.Gold * b), (long)(a.Food * b), (long)(a.Wood * b), (long)(a.Stone * b), (long)(a.Ore * b));
        }

        public static Resources operator *(decimal a, Resources b) {
            return b * a;
        }
    }
}