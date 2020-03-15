using System;
using System.Collections.Generic;
using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Markets {
    public sealed class MerchandiseTotals : ValueObject {
        public MerchandiseType Type { get; private set; }
        public int Quantity { get; private set; }
        public int Price { get; private set; }

        public MerchandiseTotals(MerchandiseType type, int quantity, int price) {
            if (quantity <= 0) throw new ArgumentOutOfRangeException("quantity", "Negative values and zero are not allowed");
            if (price <= 0) throw new ArgumentOutOfRangeException("price", "Negative values and zero are not allowed");

            Type = type;
            Quantity = quantity;
            Price = price;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return Type;
            yield return Quantity;
            yield return Price;
        }

        public Resources ToResources() {
            switch (Type) {
                case MerchandiseType.Food:
                    return new Resources(food: Quantity);
                case MerchandiseType.Wood:
                    return new Resources(wood: Quantity);
                case MerchandiseType.Stone:
                    return new Resources(stone: Quantity);
                case MerchandiseType.Ore:
                    return new Resources(ore: Quantity);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}