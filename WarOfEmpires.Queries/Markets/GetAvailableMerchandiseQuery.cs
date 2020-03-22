using WarOfEmpires.Models.Markets;

namespace WarOfEmpires.Queries.Markets {
    public sealed class GetAvailableMerchandiseQuery : IQuery<AvailableMerchandiseModel> {
        public string Email { get; }

        public GetAvailableMerchandiseQuery(string email) {
            Email = email;
        }
    }
}