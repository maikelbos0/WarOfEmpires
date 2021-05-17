namespace WarOfEmpires.Domain.Empires {
    public class Building : Entity {
        public virtual BuildingType Type { get; private set; }
        public virtual int Level { get; set; }

        protected Building() {
        }

        public Building(BuildingType type, int level) {
            Type = type;
            Level = level;
        }
    }
}