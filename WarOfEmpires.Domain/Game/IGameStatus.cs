using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Game {
    public interface IGameStatus {
        Player CurrentGrandOverlord { get; set; }
        GamePhase Phase { get; set; }
    }
}