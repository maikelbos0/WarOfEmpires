namespace WarOfEmpires.Domain.Empires {
    public class Building : Entity {
        public BuildingType Type { get; protected set; }
        public int Level { get; set; }

        protected Building() {
        }

        public Building(BuildingType type, int level) {
            Type = type;
            Level = level;
        }
    }
}