using System;
using System.Collections.Generic;
using WarOfEmpires.Domain.Attacks;

namespace WarOfEmpires.Domain.Players {
    public class TroopInfo : ValueObject {
        public Troops Troops { get; }
        public int SiegeCoverage { get; }
        public long BaseAttack { get; }
        public long BaseDefense { get; }
        public decimal TroopBonusMultiplier { get; }
        public decimal ForgeBonusMultiplier { get; }
        public decimal ArmouryBonusMultiplier { get; }

        protected TroopInfo() {
        }

        public TroopInfo(Troops troops, long baseAttack, long baseDefense, decimal troopBonusMultiplier, decimal forgeBonusMultiplier, decimal armouryBonusMultiplier, int siegeCoverage) {
            Troops = troops;
            BaseAttack = baseAttack;
            BaseDefense = baseDefense;
            TroopBonusMultiplier = troopBonusMultiplier;
            ForgeBonusMultiplier = forgeBonusMultiplier;
            ArmouryBonusMultiplier = armouryBonusMultiplier;
            SiegeCoverage = siegeCoverage;
        }

        public long GetAttackPerSoldier() {
            return (int)(BaseAttack * TroopBonusMultiplier * ForgeBonusMultiplier);

        }

        public long GetDefensePerSoldier() {
            return (int)(BaseDefense * TroopBonusMultiplier * ArmouryBonusMultiplier);
        }

        public virtual long GetTotalAttack(double? defenceModifier = null) {
            if (defenceModifier.HasValue) {
                var siegeCoverage = Math.Min(Troops.GetTotals(), SiegeCoverage);

                return (long)((siegeCoverage * 0.5 - siegeCoverage * 0.5 * defenceModifier + Troops.GetTotals() * defenceModifier) * GetAttackPerSoldier());
            }
            else {
                return GetAttackPerSoldier() * Troops.GetTotals();
            }
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