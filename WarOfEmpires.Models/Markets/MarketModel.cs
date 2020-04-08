using System.Collections.Generic;

namespace WarOfEmpires.Models.Markets {
    public sealed class MarketModel {
        public int TotalMerchants { get; set; }
        public int AvailableMerchants { get; set; }
        public int CaravanCapacity { get; set; }
        public int AvailableCapacity { get; set; }
        public List<MerchandiseModel> Merchandise { get; set; }
    }
}