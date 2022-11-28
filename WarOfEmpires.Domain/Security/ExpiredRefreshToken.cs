namespace WarOfEmpires.Domain.Security {
    public class ExpiredRefreshToken : Entity {
        public virtual byte[] Token { get; protected set; }

        protected ExpiredRefreshToken() {
        }

        public ExpiredRefreshToken(byte[] token) : this() {
            Token = token;
        }
    }
}