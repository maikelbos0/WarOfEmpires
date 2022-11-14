using WarOfEmpires.Models.Security;

namespace WarOfEmpires.Queries.Security {
    public sealed class GetUserClaimsQuery : IQuery<UserClaimsViewModel> {
        public string Email { get; }

        public GetUserClaimsQuery(string email) {
            Email = email;
        }
    }
}