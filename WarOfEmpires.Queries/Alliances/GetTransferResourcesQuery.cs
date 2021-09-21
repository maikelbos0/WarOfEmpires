using WarOfEmpires.Models.Alliances;

namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetTransferResourcesQuery : IQuery<TransferResourcesModel> {
        public string Email { get; set; }

        public GetTransferResourcesQuery(string email) {
            Email = email;
        }
    }
}
