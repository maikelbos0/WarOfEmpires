using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Siege {
    public sealed class SiegeWeaponDefinition {
        public Resources PurchaseCost { get; }
        public TroopType TroopType { get; }
        public int TroopCount { get; }
        public Resources OperatingCostPerTurn { get; }
        public int SiegeMaintenance { get; }

        public SiegeWeaponDefinition(Resources purchaseCost, TroopType troopType, int troopCount, Resources operatingCostPerTurn, int siegeMaintenance) {
            PurchaseCost = purchaseCost;
            TroopType = troopType;
            TroopCount = troopCount;
            OperatingCostPerTurn = operatingCostPerTurn;
            SiegeMaintenance = siegeMaintenance;
        }
    }
}