using System;
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

        public void Withdraw(Player player, double percentageSaved) {
            player.AddResources(MerchandiseTotals.ToResources(Type, (int)(percentageSaved * Quantity)));

            Quantity = 0;
        }

        public int Buy(Player seller, Player buyer, int requestedQuantity) {
            var quantity = Math.Min(requestedQuantity, Quantity);
            var cost = quantity * Price;
            var profit = (int)((1 - SalesTax) * cost);

            Quantity -= quantity;
            seller.AddResources(new Resources(gold: profit));
            buyer.SpendResources(new Resources(gold: cost));
            buyer.AddResources(MerchandiseTotals.ToResources(Type, quantity));

            return requestedQuantity - quantity;
        }
    }
}