using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Attacks {
    public class AttackRound : Entity {
        public virtual Attack Attack { get; protected set; }
        public virtual TroopType TroopType { get; protected set; }
        public virtual bool IsAggressor { get; protected set; }
        public virtual int Troops { get; protected set; }
        public virtual int Damage { get; protected set; }
        public virtual Casualties ArcherCasualties { get; protected set; }
        public virtual Casualties CavalryCasualties { get; protected set; }
        public virtual Casualties FootmanCasualties { get; protected set; }

        private AttackRound() { }

        internal AttackRound(Attack attack, TroopType troopType, bool isAggressor, int troops, int damage, Casualties archerCasualties, Casualties cavalryCasualties, Casualties footmanCasualties) {
            Attack = attack;
            TroopType = troopType;
            IsAggressor = isAggressor;
            Troops = troops;
            Damage = damage;
            ArcherCasualties = archerCasualties;
            CavalryCasualties = cavalryCasualties;
            FootmanCasualties = footmanCasualties;
        }
    }
}