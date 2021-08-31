using System.Collections.Generic;

namespace WarOfEmpires.Models.Markets {
    public sealed class BlackMarketModel {
        public int SellPrice { get; set; }
        public int BuyPrice { get; set; }
        public List<BlackMarketMerchandiseModel> Merchandise { get; set; }
        public string Command { get; set; }
    }
}
