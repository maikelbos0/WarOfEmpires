using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Markets {
    public sealed class MerchandiseModel {
        [DisplayName("Quantity")]
        [RegularExpression("^\\d{0,8}$", ErrorMessage = "Invalid number")]
        public int? Quantity { get; set; }
        [DisplayName("Price")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Invalid number")]
        public int? Price { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int LowestPrice { get; set; }
        public int AvailableAtLowestPrice { get; set; }
        public int TotalAvailable { get; set; }
    }
}