using WarOfEmpires.Models.Security;

namespace WarOfEmpires.Queries.Security {
    public sealed class GetUserDetailsQuery : IQuery<UserDetailsModel> {
        public string DisplayName { get; }
        public int UserId { get; }

        public GetUserDetailsQuery(string displayName, int userId) {
            DisplayName = displayName;
            UserId = userId;
        }
    }
}
