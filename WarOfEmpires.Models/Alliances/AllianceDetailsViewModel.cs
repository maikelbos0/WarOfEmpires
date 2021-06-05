using System.Collections.Generic;

namespace WarOfEmpires.Models.Alliances {
    public sealed class AllianceDetailsViewModel : EntityViewModel {
        public string Status { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int LeaderId { get; set; }
        public string Leader { get; set; }
        public List<AllianceMemberViewModel> Members { get; set; }
        public bool CanReceiveNonAggressionPactRequest { get; set; }
        public bool CanReceiveWarDeclaration { get; set; }
    }
}