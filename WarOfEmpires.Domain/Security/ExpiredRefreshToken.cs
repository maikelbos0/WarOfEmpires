namespace WarOfEmpires.Domain.Security {
    public class ExpiredRefreshToken : Entity {
        public virtual RefreshTokenFamily RefreshTokenFamily { get; protected set; }
        public virtual byte[] Token { get; set; }
    }
}