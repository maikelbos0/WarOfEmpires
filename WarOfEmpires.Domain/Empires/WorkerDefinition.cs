using WarOfEmpires.Domain.Common;

namespace WarOfEmpires.Domain.Empires {
    public sealed class WorkerDefinition {
        public WorkerType Type { get; }
        public BuildingType BuildingType { get; }
        public Resources Cost { get; }

        public WorkerDefinition(WorkerType type, BuildingType buildingType, Resources cost) {
            Type = type;
            BuildingType = buildingType;
            Cost = cost;
        }
    }
}