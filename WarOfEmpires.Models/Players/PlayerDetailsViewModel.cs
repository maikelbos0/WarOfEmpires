namespace WarOfEmpires.Models.Players {
    public sealed class PlayerDetailsViewModel : EntityViewModel {
        public int Rank { get; set; }
        public string AllianceCode { get; set; }
        public string AllianceName { get; set; }
        public string DisplayName { get; set; }
        public int Population { get; set; }
        public string Title { get; set; }
    }
}