namespace WarOfEmpires.Models.Players {
    public sealed class PlayerHomeViewModel {
        public string DisplayName { get; set; }
        public bool HasNewMessages { get; set; }
        public int NewAttackCount { get; set; }
        public int TotalSoldierCasualties { get; set; }
        public int TotalMercenaryCasualties { get; set; }
        public bool HasNewMarketSales { get; set; }
        public bool HasNewChatMessages { get; set; }
        public bool HasHousingShortage { get; set; }
        public bool WillUpkeepRunOut { get; set; }
        public bool HasUpkeepRunOut { get; set; }
        public bool HasSoldierShortage { get; set; }
        public bool HasNewInvites { get; set; }
        public int CurrentPeasants { get; set; }
    }
}
