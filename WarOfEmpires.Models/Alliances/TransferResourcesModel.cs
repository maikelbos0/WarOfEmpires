using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Alliances {
    public sealed class TransferResourcesModel {
        [DisplayName("Recipient")]
        [Required(ErrorMessage = "Recipient is required")]
        public string RecipientId { get; set; }

        public List<TransferResourcesRecipientViewModel> Recipients { get; set; }

        [DisplayName("Gold")]
        [RegularExpression("^\\d{0,8}$", ErrorMessage = "Invalid number")]
        public int? Gold { get; set; }

        [DisplayName("Food")]
        [RegularExpression("^\\d{0,8}$", ErrorMessage = "Invalid number")]
        public int? Food { get; set; }

        [DisplayName("Wood")]
        [RegularExpression("^\\d{0,8}$", ErrorMessage = "Invalid number")]
        public int? Wood { get; set; }

        [DisplayName("Stone")]
        [RegularExpression("^\\d{0,8}$", ErrorMessage = "Invalid number")]
        public int? Stone { get; set; }

        [DisplayName("Ore")]
        [RegularExpression("^\\d{0,8}$", ErrorMessage = "Invalid number")]
        public int? Ore { get; set; }
    }
}
