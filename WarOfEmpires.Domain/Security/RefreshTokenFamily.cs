using System.Collections.Generic;

namespace WarOfEmpires.Domain.Security {
    public class RefreshTokenFamily : Entity {
        public virtual ICollection<ExpiredRefreshToken> ExpiredRefreshTokens { get; set; } = new List<ExpiredRefreshToken>();
        public virtual byte[] CurrentToken { get; set; }
    }
}