namespace WarOfEmpires.Commands.Empires {
    public sealed class TrainWorkersCommand : ICommand {
        public string Email { get; }
        public string Farmers { get; }
        public string WoodWorkers { get; }
        public string StoneMasons { get; }
        public string OreMiners { get; }
        public string SiegeEngineers { get; }
        
        public TrainWorkersCommand(string email, string farmers, string woodWorkers, string stoneMasons, string oreMiners, string siegeEngineers) {
            Email = email;
            Farmers = farmers;
            WoodWorkers = woodWorkers;
            StoneMasons = stoneMasons;
            OreMiners = oreMiners;
            SiegeEngineers = siegeEngineers;
        }
    }
}