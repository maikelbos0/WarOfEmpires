using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetNewRolePlayerQuery : IQuery<NewRolePlayersModel> {
        public string Email { get; set; }
        public int RoleId { get; }

        public GetNewRolePlayerQuery(string email, int roleId) {
            Email = email;
            RoleId = roleId;
        }
    }
}