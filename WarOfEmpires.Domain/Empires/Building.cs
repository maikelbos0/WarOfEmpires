namespace WarOfEmpires.Domain.Empires {
    public sealed class Building : Entity {
        public BuildingType Type { get; private set; }
        public int Level { get; set; }

        private Building() {
        }

        public Building(BuildingType type, int level) {
            Type = type;
            Level = level;
        }
    }
}