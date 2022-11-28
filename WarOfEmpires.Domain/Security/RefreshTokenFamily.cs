using System;
using System.Collections.Generic;

namespace WarOfEmpires.Domain.Security {
    public class RefreshTokenFamily : Entity {
        public virtual ICollection<PreviousRefreshToken> PreviousRefreshTokens { get; protected set; } = new List<PreviousRefreshToken>();
        public virtual byte[] CurrentToken { get; protected set; }
        public virtual Guid RequestId { get; protected set; }

        protected RefreshTokenFamily() {
        }

        public RefreshTokenFamily(Guid requestId, byte[] token) : this() {
            CurrentToken = token;
            RequestId = requestId;
        }

        public void RotateRefreshToken(Guid requestId, byte[] newToken) {
            PreviousRefreshTokens.Add(new PreviousRefreshToken(CurrentToken));
            CurrentToken = newToken;
            RequestId = requestId;
        }
    }
}