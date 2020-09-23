using WarOfEmpires.Models.Grids;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Alliances {    
    public sealed class RoleViewModel : EntityViewModel {
        [GridColumn(0, 50, "Name")]
        public string Name { get; set; }
        public bool CanInvite { get; set; }
        [GridColumn(1, 25, "Can invite")]
        public string CanInviteString { get { return CanInvite ? "Yes" : "No"; } }
        public int Players { get; set; }
        // TODO figure out responsive behaviour when more columns are added
        [GridColumn(2, 25, "Players", SortData = nameof(Players), ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromMedium)]
        public string PlayersString { get { return Players.ToString(StringFormat.Integer); } }
    }
}