namespace WarOfEmpires.Domain.Players {
    public class PlayerBlock : Entity {
        public virtual Player BlockedPlayer { get; protected set; }

        protected PlayerBlock() {
        }

        public PlayerBlock(Player blockedPlayer) {
            BlockedPlayer = blockedPlayer;
        }
    }
}
