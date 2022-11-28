using System.Collections.Generic;

namespace WarOfEmpires.Domain.Security {
    public class RefreshTokenFamily : Entity {
        public virtual ICollection<ExpiredRefreshToken> ExpiredRefreshTokens { get; protected set; } = new List<ExpiredRefreshToken>();
        public virtual byte[] CurrentToken { get; protected set; }

        protected RefreshTokenFamily() {
        }

        public RefreshTokenFamily(byte[] token) : this() {
            CurrentToken = token;
        }

        public void RotateRefreshToken(byte[] newToken) {
            ExpiredRefreshTokens.Add(new ExpiredRefreshToken(CurrentToken));
            CurrentToken = newToken;
        }
    }
}