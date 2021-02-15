using System.Collections.Generic;

namespace WarOfEmpires.Models.Alliances {
    public sealed class AllianceDetailsViewModel : EntityViewModel {
        // TODO add can send pact request
        // TODO decide if the current player rights are part of that
        public string Code { get; set; }
        public string Name { get; set; }
        public int LeaderId { get; set; }
        public string Leader { get; set; }
        public List<AllianceMemberViewModel> Members { get; set; }
    }
}