using System;

namespace WarOfEmpires.Models.Alliances {
    public sealed class TransferResourcesRecipientViewModel : EntityViewModel {
        public int Rank { get; set; }
        public string DisplayName { get; set; }
        public string Role { get; set; }
        public DateTime? LastOnline { get; set; }
    }
}