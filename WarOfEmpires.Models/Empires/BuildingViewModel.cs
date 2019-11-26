namespace WarOfEmpires.Models.Empires {
    public sealed class BuildingViewModel {
        public int Level { get; set; }
        public string Name { get; set; }
        public ResourcesViewModel UpdateCost = new ResourcesViewModel();
    }
}