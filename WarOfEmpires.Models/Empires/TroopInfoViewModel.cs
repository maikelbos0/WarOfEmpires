namespace WarOfEmpires.Models.Empires {
    public sealed class TroopInfoViewModel {
        public ResourcesViewModel Cost { get; set; }
        public int CurrentSoldiers { get; set; }
        public int CurrentMercenaries { get; set; }
    }
}