using System;

namespace WarOfEmpires.Models.Alliances {
    public sealed class AllianceHomeMemberViewModel : EntityViewModel {
        public int Rank { get; set; }
        public string DisplayName { get; set; }
        public string Title { get; set; }
        public int Population { get; set; }
        public DateTime? LastOnline { get; set; }
    }
}