using System.Collections.Generic;

namespace WarOfEmpires.Models.Alliances {
    public sealed class AllianceHomeViewModel : EntityViewModel {
        public string Code { get; set; }
        public string Name { get; set; }
        public int LeaderId { get; set; }
        public string Leader { get; set; }
        public List<AllianceHomeMemberViewModel> Members { get; set; }
    }
}