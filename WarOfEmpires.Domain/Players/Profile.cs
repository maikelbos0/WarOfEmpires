namespace WarOfEmpires.Domain.Players {
    public class Profile : Entity {
        public virtual string FullName { get; set; }
        public virtual string Description { get; set; }
        public virtual string AvatarLocation { get; set; }
    }
}
