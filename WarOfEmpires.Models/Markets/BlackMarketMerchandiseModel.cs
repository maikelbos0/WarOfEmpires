using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Markets {
    public sealed class BlackMarketMerchandiseModel {
        [DisplayName("Quantity")]
        [RegularExpression("^\\d{0,8}$", ErrorMessage = "Invalid number")]
        public int? Quantity { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
