using System;
using WarOfEmpires.Models.Security;

namespace WarOfEmpires.Queries.Security {
    public sealed class GetUserClaimsQuery : IQuery<UserClaimsViewModel> {
        public string Email { get; }
        public Guid RequestId { get; }

        public GetUserClaimsQuery(string email, Guid requestId) {
            Email = email;
            RequestId = requestId;
        }
    }
}