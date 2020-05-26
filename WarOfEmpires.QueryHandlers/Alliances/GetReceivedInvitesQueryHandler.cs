using System.Collections.Generic;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetReceivedInvitesQueryHandler : IQueryHandler<GetReceivedInvitesQuery, IEnumerable<ReceivedInviteViewModel>> {
        public IEnumerable<ReceivedInviteViewModel> Execute(GetReceivedInvitesQuery query) {
            throw new System.NotImplementedException();
        }
    }
}