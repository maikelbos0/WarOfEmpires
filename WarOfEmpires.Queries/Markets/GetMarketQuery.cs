using WarOfEmpires.Models.Markets;

namespace WarOfEmpires.Queries.Markets {
    public sealed class GetMarketQuery : IQuery<MarketModel> {
        public string Email { get; }

        public GetMarketQuery(string email) {
            Email = email;
        }
    }
}