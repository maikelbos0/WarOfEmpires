using WarOfEmpires.Models.Grids;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Alliances {
    public sealed class AllianceViewModel : EntityViewModel {
        [GridColumn(0, 15, "Code")]
        public string Code { get; set; }
        [GridColumn(1, 40, "Name")]
        public string Name { get; set; }
        public int Members { get; set; }
        [GridColumn(2, 15, "Members", SortData = nameof(Members))]
        public string MembersString { get { return Members.ToString(StringFormat.Integer); } }
        [GridColumn(3, 30, "Leader")]
        public string Leader { get; set; }
    }
}