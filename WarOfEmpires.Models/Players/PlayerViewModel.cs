using WarOfEmpires.Models.Grids;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Players {
    [GridSorting(nameof(Rank), false)]
    public sealed class PlayerViewModel : EntityViewModel {
        public int Rank { get; set; }
        [GridColumn(0, 10, "Rank", SortData = nameof(Rank))]
        public string RankString { get { return Rank.ToString(StringFormat.Integer); } }
        [GridColumn(1, 30, "Display name")]
        public string DisplayName { get; set; }
        [GridColumn(2, 10, "Alliance")]
        public string Alliance { get; set; }
        [GridColumn(3, 25, "Title", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromMedium )]
        public string Title { get; set; }
        public int Population { get; set; }
        [GridColumn(4, 15, "Population", SortData = nameof(Population), ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string PopulationString { get { return Population.ToString(StringFormat.Integer); } }
    }
}