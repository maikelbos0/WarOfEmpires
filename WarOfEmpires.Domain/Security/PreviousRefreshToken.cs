namespace WarOfEmpires.Domain.Security {
    public class PreviousRefreshToken : Entity {
        public virtual byte[] Token { get; protected set; }

        protected PreviousRefreshToken() {
        }

        public PreviousRefreshToken(byte[] token) : this() {
            Token = token;
        }
    }
}