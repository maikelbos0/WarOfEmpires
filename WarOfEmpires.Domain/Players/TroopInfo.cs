using System.Collections.Generic;

namespace WarOfEmpires.Domain.Players {
    public sealed class TroopInfo : ValueObject {
        public int Soldiers { get; }
        public int Mercenaries { get; }
        public int BaseAttack { get; }
        public int BaseDefense { get; }
        public decimal TroopBonusMultiplier { get; }
        public decimal ForgeBonusMultiplier { get; }
        public decimal ArmouryBonusMultiplier { get; }

        public TroopInfo(int soldiers, int mercenaries, int baseAttack, int baseDefense, decimal troopBonusMultiplier, decimal forgeBonusMultiplier, decimal armouryBonusMultiplier) {
            Soldiers = soldiers;
            Mercenaries = mercenaries;
            BaseAttack = baseAttack;
            BaseDefense = baseDefense;
            TroopBonusMultiplier = troopBonusMultiplier;
            ForgeBonusMultiplier = forgeBonusMultiplier;
            ArmouryBonusMultiplier = armouryBonusMultiplier;
        }

        public int GetAttackPerSoldier() {
            return (int)(BaseAttack * TroopBonusMultiplier * ForgeBonusMultiplier);

        }

        public int GetDefensePerSoldier() {
            return (int)(BaseDefense * TroopBonusMultiplier * ArmouryBonusMultiplier);
        }

        public int GetTotalAttack() {
            return GetAttackPerSoldier() * (Soldiers + Mercenaries);
        }

        public int GetTotalDefense() {
            return GetDefensePerSoldier() * (Soldiers + Mercenaries);
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return Soldiers;
            yield return Mercenaries;
            yield return BaseAttack;
            yield return BaseDefense;
            yield return TroopBonusMultiplier;
            yield return ForgeBonusMultiplier;
            yield return ArmouryBonusMultiplier;
        }
    }
}