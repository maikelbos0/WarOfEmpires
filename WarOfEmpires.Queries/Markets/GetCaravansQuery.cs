using System.Collections.Generic;
using WarOfEmpires.Models.Markets;

namespace WarOfEmpires.Queries.Markets {
    public sealed class GetCaravansQuery : IQuery<IEnumerable<CaravanViewModel>> {
        public string Email { get; }

        public GetCaravansQuery(string email) {
            Email = email;
        }
    }
}