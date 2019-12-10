using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Attacks {
    public class AttackRound : Entity {
        public virtual Attack Attack { get; protected set; }
        public virtual TroopType TroopType { get; protected set; }
        public virtual Player Attacker { get; protected set; }
        public virtual Player Defender { get; protected set; }

        private AttackRound() { }

        internal AttackRound(Attack attack, TroopType troopType, Player attacker, Player defender) {
            Attack = attack;
            TroopType = troopType;
            Attacker = attacker;
            Defender = defender;
        }
    }
}