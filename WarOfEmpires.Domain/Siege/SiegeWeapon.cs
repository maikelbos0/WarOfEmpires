namespace WarOfEmpires.Domain.Siege {
    public class SiegeWeapon : Entity {
        public SiegeWeaponType Type { get; protected set; }
        public int Count { get; set; }

        protected SiegeWeapon() {
        }

        public SiegeWeapon(SiegeWeaponType type, int count) {
            Type = type;
            Count = count;
        }
    }
}