using System;
using System.Collections.Generic;
using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Markets {
    public sealed class BlackMarketMerchandiseTotals : ValueObject {
        public static Resources ToResources(MerchandiseType type, int quantity) {
            switch (type) {
                case MerchandiseType.Food:
                    return new Resources(food: quantity);
                case MerchandiseType.Wood:
                    return new Resources(wood: quantity);
                case MerchandiseType.Stone:
                    return new Resources(stone: quantity);
                case MerchandiseType.Ore:
                    return new Resources(ore: quantity);
                default:
                    throw new NotImplementedException();
            }
        }

        public MerchandiseType Type { get; private set; }
        public int Quantity { get; private set; }
        public int Price { get; private set; }

        public BlackMarketMerchandiseTotals(MerchandiseType type, int quantity) {
            if (quantity <= 0) throw new ArgumentOutOfRangeException("quantity", "Negative values and zero are not allowed");

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