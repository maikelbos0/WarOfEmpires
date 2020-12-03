using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetNewLeaderQuery : IQuery<NewLeadersModel> {
        public string Email { get; set; }

        public GetNewLeaderQuery(string email) {
            Email = email;
        }
    }
}