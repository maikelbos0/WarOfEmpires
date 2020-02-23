using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Domain.Attacks {
    public class TroopDefinition {
        public TroopType Type { get; }
        public BuildingType BuildingType { get; }
        public int BaseAttack { get; }
        public int BaseDefence { get; }
        public Resources Cost { get; }
        public string Name { get; }

        public TroopDefinition(TroopType type, BuildingType buildingType, int baseAttack, int baseDefence, Resources cost, string name) {
            Type = type;
            BuildingType = buildingType;
            BaseAttack = baseAttack;
            BaseDefence = baseDefence;
            Cost = cost;
            Name = name;
        }
    }
}