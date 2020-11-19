namespace WarOfEmpires.Commands.Markets {
    public sealed class MerchandiseInfo {
        public string Type { get; }
        public int? Quantity { get; }
        public int? Price { get; }

        public MerchandiseInfo(string type, int? quantity, int? price) {
            Type = type;
            Quantity = quantity;
            Price = price;
        }
    }
}