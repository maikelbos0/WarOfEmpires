using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Attacks {
    public sealed class ExecuteAttackModel {
        public int DefenderId { get; set; }
        public string DisplayName { get; set; }
        public int Population { get; set; }

        [DisplayName("Number of turns")]
        [Required(ErrorMessage = "Number of turns is required")]
        [RegularExpression("^([1-9]|10)$", ErrorMessage = "Number of turns must be a valid number between 1 and 10")]
        public string Turns { get; set; }
    }
}