using System.Collections.Generic;

namespace WarOfEmpires.Models.Alliances {
    public sealed class NewRolePlayersModel : EntityViewModel {
        public string Name { get; set; }
        public List<NewRolePlayerModel> Players { get; set; }
    }
}