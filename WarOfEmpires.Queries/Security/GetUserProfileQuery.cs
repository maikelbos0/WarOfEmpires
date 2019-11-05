using WarOfEmpires.Models.Security;

namespace WarOfEmpires.Queries.Security {
    public sealed class GetUserProfileQuery : IQuery<UserProfileModel> {
        public string Email { get; private set; }

        public GetUserProfileQuery(string email) {
            Email = email;
        }
    }
}