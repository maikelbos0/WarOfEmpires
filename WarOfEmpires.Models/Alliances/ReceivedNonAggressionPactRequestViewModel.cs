namespace WarOfEmpires.Models.Alliances {
    public sealed class ReceivedNonAggressionPactRequestViewModel : EntityViewModel {
        public int AllianceId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
