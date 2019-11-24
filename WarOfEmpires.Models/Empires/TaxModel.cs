using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Empires {
    public sealed class TaxModel {
        [RegularExpression("^[0-4]?\\d{1,2}$|^500$", ErrorMessage = "Tax must be a valid number")]
        public string Tax { get; set; }
    }
}