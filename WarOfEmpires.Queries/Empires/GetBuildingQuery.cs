using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.Queries.Empires {    
    public sealed class GetBuildingQuery : IQuery<BuildingModel> {
        public string Email { get; }
        public string BuildingType { get; }

        public GetBuildingQuery(string email, string buildingType) {
            Email = email;
            BuildingType = buildingType;
        }
    }
}