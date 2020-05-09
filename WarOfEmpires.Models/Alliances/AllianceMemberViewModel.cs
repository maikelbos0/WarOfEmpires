namespace WarOfEmpires.Models.Alliances {
    public sealed class AllianceMemberViewModel : EntityViewModel {
        public int Rank { get; set; }
        public string DisplayName { get; set; }
        public string Title { get; set; }
        public int Population { get; set; }
    }
}