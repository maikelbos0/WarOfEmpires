namespace WarOfEmpires.Models.Empires {
    public sealed class HousingTotalsViewModel {
        public int HutCapacity { get; set; }
        public int HutOccupancy { get; set; }
        public int BarracksCapacity { get; set; }
        public int BarracksOccupancy { get; set; }
        public int TotalCapacity { get; set; }
        public int TotalOccupancy { get; set; }
        public bool HasHousingShortage { get; set; }
    }
}