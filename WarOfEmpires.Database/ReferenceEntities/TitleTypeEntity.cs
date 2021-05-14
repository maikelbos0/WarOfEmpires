using System.Collections.Generic;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Database.ReferenceEntities {
    public class TitleTypeEntity : BaseReferenceEntity<TitleType> {
        public virtual ICollection<Player> Players { get; set; }
    }
}
