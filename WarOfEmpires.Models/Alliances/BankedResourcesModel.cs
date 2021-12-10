using System.ComponentModel;
using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.Models.Alliances {
    public class BankedResourcesModel {
        public int BankTurns { get; set; }
        public ResourcesViewModel BankedResources { get; set; }
        [DisplayName("Gold")]
        public int? Gold { get; set; }
        [DisplayName("Food")]
        public int? Food { get; set; }
        [DisplayName("Wood")]
        public int? Wood { get; set; }
        [DisplayName("Stone")]
        public int? Stone { get; set; }
        [DisplayName("Ore")]
        public int? Ore { get; set; }
        public string Command { get; set; }
    }
}
