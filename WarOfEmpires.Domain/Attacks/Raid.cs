using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Attacks {
    public class Raid : Attack {
        protected Raid() { }

        public Raid(Player attacker, Player defender, int turns) : base(attacker, defender, turns) {
            Type = AttackType.Raid;
        }

        public override long CalculateDamage(int stamina, bool isAggressor, TroopInfo attackerTroopInfo, Player defender) {
            return (int)(attackerTroopInfo.GetTotalAttack() * Turns * stamina / 100.0);
        }

        public override Resources GetBaseResources() {
            return Defender.Resources - new Resources(Defender.Resources.Gold);
        }

        public override bool IsSurrender() {
            return Defender.Stamina * GetArmyStrengthModifier() < DefenderMinimumStamina;
        }
    }
}