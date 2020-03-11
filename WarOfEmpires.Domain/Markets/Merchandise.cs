using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Markets {
    public sealed class Merchandise : Entity {
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

        public void SellTo(Player player, int quantity) {
            throw new System.NotImplementedException();
        }
    }
}