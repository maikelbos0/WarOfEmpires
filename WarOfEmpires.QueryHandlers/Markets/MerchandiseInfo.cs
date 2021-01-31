using WarOfEmpires.Domain.Markets;

namespace WarOfEmpires.QueryHandlers.Markets {
    public class MerchandiseInfo {
        public MerchandiseType Type { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}