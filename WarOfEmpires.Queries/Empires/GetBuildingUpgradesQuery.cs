using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.Queries.Empires {
    public sealed class GetBuildingUpgradesQuery : IQuery<BuildingUpgradesViewModel> {
        public string Email { get; }
        public string BuildingType { get; }

        public GetBuildingUpgradesQuery(string email, string buildingType) {
            Email = email;
            BuildingType = buildingType;
        }
    }
}