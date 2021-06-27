using System;

namespace WarOfEmpires.Utilities.Game {
    public interface IGameStatus {
        string CurrentGrandOverlord { get; set; }
        int? CurrentGrandOverlordId { get; set; }
        TimeSpan? CurrentGrandOverlordTime { get; set; }
    }
}