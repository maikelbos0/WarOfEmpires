using System.Collections.Generic;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Database.ReferenceEntities {
    public class RaceEntity : BaseReferenceEntity<Race> {
        public virtual ICollection<Player> Players { get; set; }
    }
}
