using System;

namespace WarOfEmpires.Domain.Markets {
    public class Transaction : Entity {
        public virtual MerchandiseType Type { get; private set; }
        public virtual int Quantity { get; private set; }
        public virtual int Price { get; private set; }
        public virtual DateTime Date { get; private set; }

        private Transaction() {
        }

        public Transaction(MerchandiseType type, int quantity, int price) {
            Type = type;
            Quantity = quantity;
            Price = price;
            Date = DateTime.UtcNow;
        }
    }
}