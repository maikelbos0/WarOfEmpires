using System.Collections.Generic;

namespace WarOfEmpires.Domain.Players {
    public sealed class ProductionInfo : ValueObject {
        public int Workers { get; }
        public decimal BuildingBonusMultiplier { get; }
        public decimal TaxRate { get; }

        public ProductionInfo(int workers, decimal buildingBonusMultiplier, decimal taxRate) {
            Workers = workers;
            BuildingBonusMultiplier = buildingBonusMultiplier;
            TaxRate = taxRate;
        }

        public int GetBaseProduction() {
            return (int)(Player.BaseResourceProduction * BuildingBonusMultiplier);
        }

        public int GetProductionPerWorker() {
            return (int)((1 - TaxRate) * GetBaseProduction());
        }

        public int GetTotalProduction() {
            return GetProductionPerWorker() * Workers;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return Workers;
            yield return BuildingBonusMultiplier;
            yield return TaxRate;
        }
    }
}