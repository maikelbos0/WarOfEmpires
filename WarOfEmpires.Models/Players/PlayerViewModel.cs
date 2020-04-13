using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.Models.Players {
    public sealed class PlayerViewModel : EntityViewModel {
        public string DisplayName { get; set; }
        public int Population { get; set; }
        public string PopulationString { get { return Population.ToString(StringFormat.Integer); } }
        public int Rank { get; set; }
        public string RankString { get { return Rank.ToString(StringFormat.Integer); } }
    }
}