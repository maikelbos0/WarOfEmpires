using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetRoleDetailsQuery : IQuery<RoleDetailsViewModel> {
        public string Email { get; set; }
        public int RoleId { get; }

        public GetRoleDetailsQuery(string email, int roleId) {
            Email = email;
            RoleId = roleId;
        }
    }
}