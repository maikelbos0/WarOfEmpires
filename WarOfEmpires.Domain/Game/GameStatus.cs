using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Game {
    public class GameStatus {
        public const int GrandOverlordHoursToWin = 72;

        public virtual int Id { get; set; }
        public virtual Player CurrentGrandOverlord { get; set; }
        public virtual GamePhase Phase { get; set; }
    }
}
