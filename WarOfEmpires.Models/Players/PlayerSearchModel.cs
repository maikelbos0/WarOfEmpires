using System.ComponentModel;

namespace WarOfEmpires.Models.Players {
    public sealed class PlayerSearchModel {
        [DisplayName("Display name")]
        public string DisplayName { get; set; }
    }
}