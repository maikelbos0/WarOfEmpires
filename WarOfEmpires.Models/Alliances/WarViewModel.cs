namespace WarOfEmpires.Models.Alliances {
    public sealed class WarViewModel : EntityViewModel {
        public int AllianceId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool PeaceDeclared { get; set; }
    }
}
