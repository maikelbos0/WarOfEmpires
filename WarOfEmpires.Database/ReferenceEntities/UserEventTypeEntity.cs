using WarOfEmpires.Domain.Security;
using System.Collections.Generic;

namespace WarOfEmpires.Database.ReferenceEntities {
    public class UserEventTypeEntity : BaseReferenceEntity<UserEventType> {
        public virtual ICollection<UserEvent> UserEvents { get; set; }
    }
}