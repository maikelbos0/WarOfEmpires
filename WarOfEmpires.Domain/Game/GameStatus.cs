using VDT.Core.DependencyInjection;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Game {
    [SingletonServiceImplementation(typeof(IGameStatus))]
    public class GameStatus : IGameStatus {
        public virtual int Id { get; set; }
        public virtual Player CurrentGrandOverlord { get; set; }
        public virtual GamePhase Phase { get; set; }
    }
}
