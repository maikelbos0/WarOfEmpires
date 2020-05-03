using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Alliances {
    public sealed class AllianceViewModel : EntityViewModel {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Members { get; set; }
        public string MembersString { get { return Members.ToString(StringFormat.Integer); } }
        public string Leader { get; set; }
    }
}