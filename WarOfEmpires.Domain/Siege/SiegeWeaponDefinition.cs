using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Siege {
    public sealed class SiegeWeaponDefinition {
        public Resources Cost { get; }
        public TroopType TroopType { get; }
        public int TroopCount { get; }
        public double ChanceToDestroy { get; }
        public string Name { get; }
        public string Description { get; }

        public SiegeWeaponDefinition(Resources cost, TroopType troopType, int troopCount, double chanceToDestroy, string name, string description) {
            Cost = cost;
            TroopType = troopType;
            TroopCount = troopCount;
            ChanceToDestroy = chanceToDestroy;
            Name = name;
            Description = description;
        }
    }
}