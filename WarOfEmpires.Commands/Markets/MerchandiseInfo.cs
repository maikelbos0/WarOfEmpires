namespace WarOfEmpires.Commands.Markets {
    public sealed class MerchandiseInfo {
        public string Type { get; }
        public string Quantity { get; }
        public string Price { get; }

        public MerchandiseInfo(string type, string quantity, string price) {
            Type = type;
            Quantity = quantity;
            Price = price;
        }
    }
}