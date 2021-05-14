using WarOfEmpires.Domain.Security;
using System.Collections.Generic;

namespace WarOfEmpires.Database.ReferenceEntities {
    public class UserStatusEntity : BaseReferenceEntity<UserStatus> {
        public virtual ICollection<User> Users { get; set; }
    }
}