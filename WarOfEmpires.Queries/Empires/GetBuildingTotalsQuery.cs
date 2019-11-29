using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.Queries.Empires {
    public sealed class GetBuildingTotalsQuery : IQuery<BuildingTotalsViewModel> {
        public string Email { get; }

        public GetBuildingTotalsQuery(string email) {
            Email = email;
        }
    }
}