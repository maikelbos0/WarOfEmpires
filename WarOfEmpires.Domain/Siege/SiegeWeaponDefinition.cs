using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Siege {
    public sealed class SiegeWeaponDefinition {
        public Resources PurchaseCost { get; }
        public TroopType TroopType { get; }
        public int TroopCount { get; }
        public double ChanceToDestroy { get; }
        public string Description { get; }

        public SiegeWeaponDefinition(Resources purchaseCost, TroopType troopType, int troopCount, double chanceToDestroy, string description) {
            PurchaseCost = purchaseCost;
            TroopType = troopType;
            TroopCount = troopCount;
            ChanceToDestroy = chanceToDestroy;
            Description = description;
        }
    }
}