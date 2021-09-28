using WarOfEmpires.Models.Players;

namespace WarOfEmpires.Queries.Players {
    public sealed class GetProfileQuery : IQuery<ProfileModel> {
        public string Email { get; }

        public GetProfileQuery(string email) {
            Email = email;
        }
    }
}
