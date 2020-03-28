﻿namespace WarOfEmpires.Models.Players {
    public sealed class NotificationsViewModel {
        public bool HasNewMessages { get; set; }
        public bool HasNewAttacks { get; set; }
        public bool HasNewSales { get; set; }
        public bool HasHousingShortage { get; set; }
        public bool HasUpkeepShortage { get; set; }
        public bool HasSoldierShortage { get; set; }
    }
}