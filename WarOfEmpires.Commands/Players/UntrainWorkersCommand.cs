namespace WarOfEmpires.Commands.Players {
    public sealed class UntrainWorkersCommand : ICommand {
        public string Email { get; }
        public string Farmers { get; }
        public string WoodWorkers { get; }
        public string StoneMasons { get; }
        public string OreMiners { get; }

        public UntrainWorkersCommand(string email, string farmers, string woodWorkers, string stoneMasons, string oreMiners) {
            Email = email;
            Farmers = farmers;
            WoodWorkers = woodWorkers;
            StoneMasons = stoneMasons;
            OreMiners = oreMiners;
        }
    }
}