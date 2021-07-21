using System;

namespace WarOfEmpires.Models.Security {
    public sealed class UserViewModel : EntityViewModel {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string AllianceName { get; set; }
        public string AllianceCode { get; set; }
        public string Status { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime? LastOnline { get; set; }
    }
}
