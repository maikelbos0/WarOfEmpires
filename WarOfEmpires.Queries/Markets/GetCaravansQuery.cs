using WarOfEmpires.Models.Markets;

namespace WarOfEmpires.Queries.Markets {
    public sealed class GetCaravansQuery : IQuery<CaravansModel> {
        public string Email { get; }

        public GetCaravansQuery(string email) {
            Email = email;
        }
    }
}