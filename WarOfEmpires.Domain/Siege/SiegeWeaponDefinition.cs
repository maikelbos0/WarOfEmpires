using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Siege {
    public sealed class SiegeWeaponDefinition {
        public SiegeWeaponType Type { get; }
        public Resources Cost { get; }
        public TroopType TroopType { get; }
        public int TroopCount { get; }
        public double ChanceToDestroy { get; }
        public int Maintenance { get; }
        public string Name { get; }
        public string Description { get; }

        public SiegeWeaponDefinition(SiegeWeaponType type, Resources cost, TroopType troopType, int troopCount, double chanceToDestroy, int maintenance, string name, string description) {
            Type = type;
            Cost = cost;
            TroopType = troopType;
            TroopCount = troopCount;
            ChanceToDestroy = chanceToDestroy;
            Maintenance = maintenance;
            Name = name;
            Description = description;
        }
    }
}