using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Siege {
    public class SiegeWeapon : Entity {
        public Player Player { get; protected set; }
        public SiegeWeaponType Type { get; protected set; }
        public int Count { get; set; }

        protected SiegeWeapon() {
        }

        public SiegeWeapon(Player player, SiegeWeaponType type) {
            Player = player;
            Type = type;
        }
    }
}