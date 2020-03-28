using System;

namespace WarOfEmpires.Domain.Markets {
    public class Transaction : Entity {
        public virtual MerchandiseType Type { get; protected set; }
        public virtual int Quantity { get; protected set; }
        public virtual int Price { get; protected set; }
        public virtual DateTime Date { get; protected set; }
        public virtual bool IsRead { get; set; }

        protected Transaction() {
        }

        public Transaction(MerchandiseType type, int quantity, int price) {
            Type = type;
            Quantity = quantity;
            Price = price;
            Date = DateTime.UtcNow;
        }
    }
}