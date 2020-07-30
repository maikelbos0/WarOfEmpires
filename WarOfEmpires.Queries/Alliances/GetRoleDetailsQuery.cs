using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetRoleDetailsQuery : IQuery<RoleDetailsViewModel> {
        public string Email { get; set; }
        public string RoleId { get; }

        public GetRoleDetailsQuery(string email, string roleId) {
            Email = email;
            RoleId = roleId;
        }
    }
}