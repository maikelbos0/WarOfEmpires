using System;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Security {
    public sealed class UserViewModel : EntityViewModel {
        [GridColumn(0, 20, "Email")]
        public string Email { get; set; }
        [GridColumn(1, 20, "Display name")]
        public string DisplayName { get; set; }
        [GridColumn(2, 10, "Alliance code")]
        public string AllianceCode { get; set; }
        [GridColumn(3, 20, "Alliance name")]
        public string AllianceName { get; set; }
        [GridColumn(4, 10, "Status")]
        public string Status { get; set; }
        public bool IsAdmin { get; set; }
        [GridColumn(5, 10, "Is admin")]
        public string IsAdminString { get { return IsAdmin ? "Yes" : "No"; } }
        public DateTime? LastOnline { get; set; }
        [GridColumn(6, 10, "Last online")]
        public string LastOnlineString { get { return LastOnline?.ToString(StringFormat.Date); } }
    }
}
