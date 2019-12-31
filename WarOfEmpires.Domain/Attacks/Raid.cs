using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Attacks {
    public class Raid : Attack {
        protected Raid() { }

        public Raid(Player attacker, Player defender, int turns) : base(attacker, defender, turns) {
            Type = AttackType.Raid;
        }

        public override long CalculateDamage(int stamina, TroopInfo attackerTroopInfo, Player defender) {
            // We first multiply and only last divide to get the most accurate values without resorting to decimals
            return attackerTroopInfo.GetTotalAttack() * stamina * Turns / 100;
        }

        public override Resources GetBaseResources() {
            return Defender.Resources - new Resources(Defender.Resources.Gold);
        }
    }
}