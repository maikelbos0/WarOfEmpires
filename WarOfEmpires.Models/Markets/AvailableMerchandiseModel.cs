using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Markets {
    public sealed class AvailableMerchandiseModel {
        [DisplayName("Food")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Food must be a valid number")]
        public string Food { get; set; }
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Food price must be a valid number")]
        public string FoodPrice { get; set; }
        [DisplayName("Wood")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Wood must be a valid number")]
        public string Wood { get; set; }
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Wood price must be a valid number")]
        public string WoodPrice { get; set; }
        [DisplayName("Stone")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Stone must be a valid number")]
        public string Stone { get; set; }
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Stone price must be a valid number")]
        public string StonePrice { get; set; }
        [DisplayName("Ore")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Ore must be a valid number")]
        public string Ore { get; set; }
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Ore price must be a valid number")]
        public string OrePrice { get; set; }
        public MerchandiseInfoViewModel FoodInfo { get; set; }
        public MerchandiseInfoViewModel WoodInfo { get; set; }
        public MerchandiseInfoViewModel StoneInfo { get; set; }
        public MerchandiseInfoViewModel OreInfo { get; set; }
    }
}