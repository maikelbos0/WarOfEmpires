using WarOfEmpires.Domain.Security;
using System.Collections.Generic;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal class UserStatusEntity : BaseReferenceEntity<UserStatus> {
        public virtual ICollection<User> Users { get; set; }
    }
}