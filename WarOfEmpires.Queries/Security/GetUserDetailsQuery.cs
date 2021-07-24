using WarOfEmpires.Models.Security;

namespace WarOfEmpires.Queries.Security {
    public sealed class GetUserDetailsQuery : IQuery<UserDetailsModel> {
        public int Id { get; }

        public GetUserDetailsQuery(int id) {
            Id = id;
        }
    }
}
