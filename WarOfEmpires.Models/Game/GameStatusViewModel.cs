using System;

namespace WarOfEmpires.Models.Game {
    public sealed class GameStatusViewModel {
        public string Phase { get; set; }
        public int? CurrentGrandOverlordId { get; set; }
        public string CurrentGrandOverlord { get; set; }
        public TimeSpan? CurrentGrandOverlordTime { get; set; }
        public int GrandOverlordHoursToWin { get; set; }
    }
}
