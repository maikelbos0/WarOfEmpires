using System.Collections.Generic;
using WarOfEmpires.Domain.Game;

namespace WarOfEmpires.Database.ReferenceEntities {
    public class GamePhaseEntity : BaseReferenceEntity<GamePhase> {
        public virtual ICollection<GameStatus> GameStatus { get; set; }
    }
}
