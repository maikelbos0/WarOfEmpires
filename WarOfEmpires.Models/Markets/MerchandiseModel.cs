﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Markets {
    public sealed class MerchandiseModel {
        [DisplayName("Quantity")]
        [RegularExpression("^\\d{0,8}$", ErrorMessage = "Invalid number")]
        public string Quantity { get; set; }
        [DisplayName("Price")]
        [RegularExpression("^\\d{0,6}$", ErrorMessage = "Invalid number")]
        public string Price { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        [DisplayName("Minimum price")]
        public int LowestPrice { get; set; }
        [DisplayName("Available at minimum price")]
        public int AvailableAtLowestPrice { get; set; }
        [DisplayName("Total available")]
        public int TotalAvailable { get; set; }
    }
}