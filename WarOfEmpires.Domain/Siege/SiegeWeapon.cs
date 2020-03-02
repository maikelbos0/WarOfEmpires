namespace WarOfEmpires.Domain.Siege {
    public sealed class SiegeWeapon : Entity {
        public SiegeWeaponType Type { get; private set; }
        public int Count { get; set; }

        private SiegeWeapon() {
        }

        public SiegeWeapon(SiegeWeaponType type, int count) {
            Type = type;
            Count = count;
        }
    }
}