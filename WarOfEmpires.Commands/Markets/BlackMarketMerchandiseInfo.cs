namespace WarOfEmpires.Commands.Markets {
    public sealed class BlackMarketMerchandiseInfo {
        public string Type { get; }
        public int? Quantity { get; }

        public BlackMarketMerchandiseInfo(string type, int? quantity) {
            Type = type;
            Quantity = quantity;
        }
    }
}
