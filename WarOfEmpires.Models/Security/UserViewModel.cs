using System;
using WarOfEmpires.Models.Formatting;
using WarOfEmpires.Models.Grids;

namespace WarOfEmpires.Models.Security {
    public sealed class UserViewModel : EntityViewModel {
        [GridColumn(0, 25, "Email", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromLarge)]
        public string Email { get; set; }
        [GridColumn(1, 15, "Display name")]
        public string DisplayName { get; set; }
        [GridColumn(2, 10, "Alliance code")]
        public string AllianceCode { get; set; }
        [GridColumn(3, 15, "Alliance name", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string AllianceName { get; set; }
        [GridColumn(4, 10, "Status")]
        public string Status { get; set; }
        public bool IsAdmin { get; set; }
        [GridColumn(5, 10, "Is admin")]
        public string IsAdminString { get { return IsAdmin ? "Yes" : "No"; } }
        public DateTime? LastOnline { get; set; }
        [GridColumn(6, 15, "Last online", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromMedium)]
        public string LastOnlineString { get { return LastOnline?.ToString(StringFormat.Date); } }
    }
}
