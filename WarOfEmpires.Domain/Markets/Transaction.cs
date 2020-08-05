﻿using System;

namespace WarOfEmpires.Domain.Markets {
    public sealed class Transaction : Entity {
        public MerchandiseType Type { get; private set; }
        public int Quantity { get; private set; }
        public int Price { get; private set; }
        public DateTime Date { get; private set; }

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