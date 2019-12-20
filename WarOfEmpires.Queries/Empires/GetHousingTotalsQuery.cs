using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.Queries.Empires {
    public sealed class GetHousingTotalsQuery : IQuery<HousingTotalsViewModel> {
        public string Email { get; }

        public GetHousingTotalsQuery(string email) {
            Email = email;
        }
    }
}