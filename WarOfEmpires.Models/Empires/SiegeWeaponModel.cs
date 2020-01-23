namespace WarOfEmpires.Models.Empires {
    public class SiegeWeaponModel {
        public string Name { get; set; }
        public ResourcesViewModel Cost { get; set; }
        public int TroopCount { get; set; }
        public int Maintenance { get; set; }
        public string Description { get; set; }
        public int CurrentCount { get; set; }
        public int CurrentTroopCount { get; set; }
        public string Count { get; set; }
    }
}