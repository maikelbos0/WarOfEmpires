using System.Collections.Generic;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal sealed class TitleTypeEntity : BaseReferenceEntity<TitleType> {
        public ICollection<Player> Players { get; set; }
    }
}
