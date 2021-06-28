using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Game {
    public sealed class GamePhaseModel {
        [DisplayName("Game phase")]
        [Required(ErrorMessage = "Game phase is required")]
        public string Phase { get; set; }
    }
}
