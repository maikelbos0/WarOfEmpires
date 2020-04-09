using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Markets {
    public sealed class MerchandiseModel {
        [RegularExpression("^\\d{0,8}$", ErrorMessage = "Invalid number")]
        public string Quantity { get; set; }
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Invalid number")]
        public string Price { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int LowestPrice { get; set; }
        public int AvailableAtLowestPrice { get; set; }
        public int TotalAvailable { get; set; }
    }
}