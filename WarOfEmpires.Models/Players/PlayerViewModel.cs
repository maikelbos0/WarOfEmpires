using WarOfEmpires.Models.Grids;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Players {
    [GridSorting(nameof(Rank), false)]
    public sealed class PlayerViewModel : EntityViewModel {
        public int Rank { get; set; }
        [GridColumn(0, 15, "Rank", SortData = nameof(Rank))]
        public string RankString { get { return Rank.ToString(StringFormat.Integer); } }
        [GridColumn(1, 40, "Display name")]
        public string DisplayName { get; set; }
        [GridColumn(2, 30, "Title")]
        public string Title { get; set; }
        public int Population { get; set; }
        [GridColumn(3, 15, "Population", SortData = nameof(Population))]
        public string PopulationString { get { return Population.ToString(StringFormat.Integer); } }
    }
}