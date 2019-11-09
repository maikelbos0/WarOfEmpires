namespace WarOfEmpires.Domain.Players {
    public class Player : AggregateRoot {
        public virtual string DisplayName { get; protected set; }
        public virtual Security.User User { get; protected set; }

        protected Player() {
        }

        public Player(int id, string displayName) {
            Id = id;
            DisplayName = displayName;
        }
    }
}