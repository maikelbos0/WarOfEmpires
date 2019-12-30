namespace WarOfEmpires.Models.Empires {
    public sealed class ResourceHeaderViewModel {
        public ResourcesViewModel Resources { get; set; }
        public ResourcesViewModel BankedResources { get; set; }
        public int AttackTurns { get; set; }
        public int BankTurns { get; set; }
    }
}