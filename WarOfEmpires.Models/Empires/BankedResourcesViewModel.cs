namespace WarOfEmpires.Models.Empires {
    public sealed class BankedResourcesViewModel {
        public ResourcesViewModel BankedResources { get; set; }
        public ResourcesViewModel Capacity { get; set; }
        public ResourcesViewModel AvailableCapacity { get; set; }
        public ResourcesViewModel BankableResources { get; set; }
    }
}