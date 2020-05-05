namespace WarOfEmpires.Models.Players {
    public sealed class CurrentPlayerViewModel {
        public bool IsAuthenticated { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsInAlliance { get; set; }
        public string UserName { get; set; }
    }
}