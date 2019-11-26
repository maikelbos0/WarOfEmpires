using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Empires {
    public class Building : Entity {
        public Player Player { get; protected set; }
        public BuildingType Type { get; protected set; }
        public int Level { get; set; }

        protected Building() {
        }

        public Building(Player player, BuildingType type, int level) {
            Player = player;
            Type = Type;
            Level = level;
        }
    }
}