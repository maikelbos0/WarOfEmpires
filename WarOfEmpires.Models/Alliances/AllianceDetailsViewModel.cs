using System.Collections.Generic;

namespace WarOfEmpires.Models.Alliances {
    public sealed class AllianceDetailsViewModel : EntityViewModel {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<AllianceMemberViewModel> Members { get; set; }
    }
}