namespace WarOfEmpires.Domain.Players {
    public class Player : AggregateRoot {
        public virtual string DisplayName { get; protected set; }
        public Security.User User { get; protected set; }

        protected Player() {
        }

        public Player(Security.User user, string displayName) {
            User = user;
            DisplayName = displayName;
        }
    }
}