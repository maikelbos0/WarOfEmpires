using WarOfEmpires.Domain.Security;
using System.Collections.Generic;

namespace WarOfEmpires.Database.ReferenceEntities {
    internal sealed class UserStatusEntity : BaseReferenceEntity<UserStatus> {
        public ICollection<User> Users { get; set; }
    }
}