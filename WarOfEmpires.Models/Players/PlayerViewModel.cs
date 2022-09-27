using System;
using WarOfEmpires.Models.Formatting;
using WarOfEmpires.Models.Grids;

namespace WarOfEmpires.Models.Players {
    [GridSorting(nameof(Rank), false)]
    public sealed class PlayerViewModel : EntityViewModel {
        [GridColumn(0, 10, "Status")]
        public string Status { get; set; }
        public int Rank { get; set; }
        [GridColumn(1, 10, "Rank", SortData = nameof(Rank))]
        public string RankString { get { return Rank.ToString(StringFormat.Integer); } }
        [GridColumn(2, 20, "Display name")]
        public string DisplayName { get; set; }
        [GridColumn(3, 10, "Race", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromLarge)]
        public string Race { get; set; }
        [GridColumn(4, 10, "Alliance")]
        public string Alliance { get; set; }
        [GridColumn(5, 20, "Title", ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromMedium )]
        public string Title { get; set; }
        public int Population { get; set; }
        [GridColumn(6, 10, "Population", SortData = nameof(Population), ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromSmall)]
        public string PopulationString { get { return Population.ToString(StringFormat.Integer); } }
        public TimeSpan GrandOverlordTime { get; set; }
        [GridColumn(7, 10, "GO Time", SortData = nameof(GrandOverlordTime), ResponsiveDisplayBehaviour = ResponsiveDisplayBehaviour.HiddenFromMedium)]
        public string GrandOverlordTimeString { get { return GrandOverlordTime > TimeSpan.Zero ? $"{(int)GrandOverlordTime.TotalHours}:{GrandOverlordTime.Minutes:00}" : null; } }
    }
}
