using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Attacks {
    public class Assault : Attack {
        protected Assault() { }

        public Assault(Player attacker, Player defender, int turns) : base(attacker, defender, turns) {
            Type = AttackType.Assault;
        }

        public override long CalculateDamage(int stamina, TroopInfo attackerTroopInfo, Player defender) {
            // TODO take defences into account
            // We first multiply and only last divide to get the most accurate values without resorting to decimals
            return attackerTroopInfo.GetTotalAttack() * stamina * Turns / 100;
        }

        public override Resources GetBaseResources() {
            return new Resources(Defender.Resources.Gold);
        }
    }
}