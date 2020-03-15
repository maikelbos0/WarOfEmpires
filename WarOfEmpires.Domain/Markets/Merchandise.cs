using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Markets {
    public sealed class Merchandise : Entity {
        public const double SalesTax = 0.15;

        public MerchandiseType Type { get; private set; }
        public int Quantity { get; private set; }
        public int Price { get; private set; }

        private Merchandise() {
        }

        public Merchandise(MerchandiseType type, int quantity, int price) {
            Type = type;
            Quantity = quantity;
            Price = price;
        }

        public void Buy(Player seller, Player buyer, int quantity) {
            var cost = quantity * Price;
            var profit = (int)((1 - SalesTax) * cost);

            Quantity -= quantity;
            seller.AddResources(new Resources(gold: profit));
            buyer.SpendResources(new Resources(gold: cost));
            buyer.AddResources(MerchandiseTotals.ToResources(Type, quantity));
        }
    }
}