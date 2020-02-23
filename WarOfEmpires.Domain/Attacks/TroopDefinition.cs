using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Attacks {
    public class TroopDefinition {
        public TroopType Type { get; }
        public Resources Cost { get; }
        public string Name { get; }

        public TroopDefinition(TroopType type, Resources cost, string name) {
            Type = type;
            Cost = cost;
            Name = name;
        }
    }
}