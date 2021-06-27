using System;
using VDT.Core.DependencyInjection;

namespace WarOfEmpires.Utilities.Game {
    [SingletonServiceImplementation(typeof(IGameStatus))]
    public sealed class GameStatus : IGameStatus {
        public int? CurrentGrandOverlordId { get; set; }
        public string CurrentGrandOverlord { get; set; }
        public TimeSpan? CurrentGrandOverlordTime { get; set; }
    }
}
