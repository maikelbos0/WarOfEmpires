using System.Collections.Generic;

namespace WarOfEmpires.Domain.Players {
    // TODO Write tests
    public sealed class TroopInfo : ValueObject {
        public Troops Troops { get; }
        public long BaseAttack { get; }
        public long BaseDefense { get; }
        public decimal TroopBonusMultiplier { get; }
        public decimal ForgeBonusMultiplier { get; }
        public decimal ArmouryBonusMultiplier { get; }

        public TroopInfo(Troops troops, long baseAttack, long baseDefense, decimal troopBonusMultiplier, decimal forgeBonusMultiplier, decimal armouryBonusMultiplier) {
            Troops = troops;
            BaseAttack = baseAttack;
            BaseDefense = baseDefense;
            TroopBonusMultiplier = troopBonusMultiplier;
            ForgeBonusMultiplier = forgeBonusMultiplier;
            ArmouryBonusMultiplier = armouryBonusMultiplier;
        }

        public long GetAttackPerSoldier() {
            return (int)(BaseAttack * TroopBonusMultiplier * ForgeBonusMultiplier);

        }

        public long GetDefensePerSoldier() {
            return (int)(BaseDefense * TroopBonusMultiplier * ArmouryBonusMultiplier);
        }

        public long GetTotalAttack() {
            return GetAttackPerSoldier() * Troops.GetTotals();
        }

        public long GetTotalDefense() {
            return GetDefensePerSoldier() * Troops.GetTotals();
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return Troops.Soldiers;
            yield return Troops.Mercenaries;
            yield return BaseAttack;
            yield return BaseDefense;
            yield return TroopBonusMultiplier;
            yield return ForgeBonusMultiplier;
            yield return ArmouryBonusMultiplier;
        }
    }
}