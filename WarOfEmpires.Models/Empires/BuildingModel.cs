namespace WarOfEmpires.Models.Empires {
    public sealed class BuildingModel {
        public string BuildingType { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ResourcesViewModel UpdateCost { get; set; } = new ResourcesViewModel();
    }
}