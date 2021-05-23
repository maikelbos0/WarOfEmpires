using System;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Markets {
    public class Merchandise : Entity {
        public const double SalesTax = 0.15;

        public virtual MerchandiseType Type { get; private set; }
        public virtual int Quantity { get; private set; }
        public virtual int Price { get; private set; }

        protected Merchandise() {
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
            var transaction = new Transaction(Type, quantity, Price);

            Quantity -= quantity;
            seller.AddResources(new Resources(gold: profit));
            seller.SellTransactions.Add(transaction);
            seller.HasNewMarketSales = true;
            buyer.SpendResources(new Resources(gold: cost));
            buyer.AddResources(MerchandiseTotals.ToResources(Type, quantity));
            buyer.BuyTransactions.Add(transaction);

            return requestedQuantity - quantity;
        }
    }
}