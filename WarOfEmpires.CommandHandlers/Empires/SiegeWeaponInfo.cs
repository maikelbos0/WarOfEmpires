using WarOfEmpires.Domain.Siege;

namespace WarOfEmpires.CommandHandlers.Empires {
    internal class SiegeWeaponInfo {
        public SiegeWeaponType Type { get; }
        public int Count { get; }

        public SiegeWeaponInfo(SiegeWeaponType type, int count) {
            Type = type;
            Count = count;
        }
    }
}