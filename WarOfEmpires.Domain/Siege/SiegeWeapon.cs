namespace WarOfEmpires.Domain.Siege {
    public class SiegeWeapon : Entity {
        public virtual SiegeWeaponType Type { get; private set; }
        public virtual int Count { get; set; }

        protected SiegeWeapon() {
        }

        public SiegeWeapon(SiegeWeaponType type, int count) {
            Type = type;
            Count = count;
        }
    }
}