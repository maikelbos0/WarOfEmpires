using System;

namespace WarOfEmpires.Models.Players {
    public sealed class VictoryStatusViewModel {
        public int? CurrentGrandOverlordId { get; set; }
        public string CurrentGrandOverlord { get; set; }
        public TimeSpan? GrandOverlordTime { get; set; }
    }
}
