using System;
using System.Collections.Generic;
using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Markets {
    public sealed class BlackMarketMerchandiseTotals : ValueObject {
        public static Resources ToResources(MerchandiseType type, int quantity) {
            return type switch {
                MerchandiseType.Food => new Resources(food: quantity),
                MerchandiseType.Wood => new Resources(wood: quantity),
                MerchandiseType.Stone => new Resources(stone: quantity),
                MerchandiseType.Ore => new Resources(ore: quantity),
                _ => throw new NotImplementedException(),
            };
        }

        public MerchandiseType Type { get; private set; }
        public int Quantity { get; private set; }
        public int Price { get; private set; }

        public BlackMarketMerchandiseTotals(MerchandiseType type, int quantity) {
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Negative values and zero are not allowed");

            Type = type;
            Quantity = quantity;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return Type;
            yield return Quantity;
        }

        public Resources ToResources() {
            return ToResources(Type, Quantity);
        }
    }
}