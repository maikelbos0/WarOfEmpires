using System.Collections.Generic;

namespace WarOfEmpires.Domain.Players {
    public sealed class ProductionInfo : ValueObject {
        public int Workers { get; }
        public decimal BonusMultiplier { get; }
        public decimal TaxRate { get; }

        public ProductionInfo(int workers, decimal bonusMultiplier, decimal taxRate) {
            Workers = workers;
            BonusMultiplier = bonusMultiplier;
            TaxRate = taxRate;
        }

        public int GetBaseProduction() {
            return (int)(Player.BaseResourceProduction * BonusMultiplier);
        }

        public int GetProductionPerWorker() {
            return (int)((1 - TaxRate) * GetBaseProduction());
        }

        public int GetTotalProduction() {
            return GetProductionPerWorker() * Workers;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return Workers;
            yield return BonusMultiplier;
            yield return TaxRate;
        }
    }
}