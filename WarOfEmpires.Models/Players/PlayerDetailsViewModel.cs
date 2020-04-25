namespace WarOfEmpires.Models.Players {
    public sealed class PlayerDetailsViewModel : EntityViewModel {
        public string DisplayName { get; set; }
        public int Population { get; set; }
        public int Rank { get; set; }
        public string Title { get; set; }
    }
}