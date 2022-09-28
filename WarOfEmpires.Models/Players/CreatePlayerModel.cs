using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Players {
    public sealed class CreatePlayerModel {
        public List<RaceViewModel> Races { get; set; } = new List<RaceViewModel>();

        [DisplayName("Race")]
        [Required(ErrorMessage = "Race is required")]
        public string Race { get; set; }

        [DisplayName("Display name")]
        [Required(ErrorMessage = "Display name is required")]
        [MaxLength(25, ErrorMessage = "Display name can only be 25 characters long")]
        public string DisplayName { get; set; }

        [DisplayName("Full name")]
        [MaxLength(100, ErrorMessage = "Full name can only be 100 characters long")]
        public string FullName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }
    }
}
