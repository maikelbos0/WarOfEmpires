using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetNewRolePlayerQuery : IQuery<NewRolePlayerModel> {
        public string Email { get; set; }
        public string RoleId { get; }

        public GetNewRolePlayerQuery(string email, string roleId) {
            Email = email;
            RoleId = roleId;
        }
    }
}