using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.Queries.Empires {
    public sealed class GetTaxQuery : IQuery<TaxModel> {
        public string Email { get; }

        public GetTaxQuery(string email) {
            Email = email;
        }
    }
}