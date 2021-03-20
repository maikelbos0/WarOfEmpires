using WarOfEmpires.Models.Grids;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Alliances {
    public sealed class AllianceViewModel : EntityViewModel {
        [GridColumn(0, 15, "Status")]
        public string Status { get; set; }
        [GridColumn(1, 15, "Code")]
        public string Code { get; set; }
        [GridColumn(2, 30, "Name")]
        public string Name { get; set; }
        public int Members { get; set; }
        [GridColumn(3, 15, "Members", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall, SortData = nameof(Members))]
        public string MembersString { get { return Members.ToString(StringFormat.Integer); } }
        [GridColumn(4, 25, "Leader", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromMedium)]
        public string Leader { get; set; }
    }
}