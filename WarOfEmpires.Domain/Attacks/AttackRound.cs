using System.Collections.Generic;

namespace WarOfEmpires.Domain.Attacks {
    public class AttackRound : Entity {
        public virtual TroopType TroopType { get; protected set; }
        public virtual bool IsAggressor { get; protected set; }
        public virtual int Troops { get; protected set; }
        public virtual long Damage { get; protected set; }
        public virtual ICollection<Casualties> Casualties { get; protected set; } = new List<Casualties>();

        protected AttackRound() { }

        internal AttackRound(TroopType troopType, bool isAggressor, int troops, long damage, ICollection<Casualties> casualties) {
            TroopType = troopType;
            IsAggressor = isAggressor;
            Troops = troops;
            Damage = damage;
            Casualties = casualties;
        }
    }
}