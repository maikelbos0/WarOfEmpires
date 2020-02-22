using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Attacks {
    public class AttackRound : Entity {
        public virtual Attack Attack { get; protected set; }
        public virtual TroopType TroopType { get; protected set; }
        public virtual bool IsAggressor { get; protected set; }
        public virtual int Troops { get; protected set; }
        public virtual long Damage { get; protected set; }
        // TODO make this a list
        public virtual Casualties Casualties { get; protected set; }

        protected AttackRound() { }

        internal AttackRound(Attack attack, TroopType troopType, bool isAggressor, int troops, long damage, Casualties casualties) {
            Attack = attack;
            TroopType = troopType;
            IsAggressor = isAggressor;
            Troops = troops;
            Damage = damage;
            Casualties = casualties;
        }
    }
}