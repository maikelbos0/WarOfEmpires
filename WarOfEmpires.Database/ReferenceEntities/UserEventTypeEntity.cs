using WarOfEmpires.Domain.Security;
using System.Collections.Generic;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal sealed class UserEventTypeEntity : BaseReferenceEntity<UserEventType> {
        public ICollection<UserEvent> UserEvents { get; set; }
    }
}