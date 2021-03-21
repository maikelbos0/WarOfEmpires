using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetReceivedNonAggressionPactRequestsQueryHandler : IQueryHandler<GetReceivedNonAggressionPactRequestsQuery, IEnumerable<ReceivedNonAggressionPactRequestViewModel>> {
        private readonly IWarContext _context;

        public GetReceivedNonAggressionPactRequestsQueryHandler(IWarContext context) {
            _context = context;
        }

        public IEnumerable<ReceivedNonAggressionPactRequestViewModel> Execute(GetReceivedNonAggressionPactRequestsQuery query) {
            return _context.Players
                .Include(p => p.Alliance.ReceivedNonAggressionPactRequests.Select(r => r.Sender))
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance
                .ReceivedNonAggressionPactRequests
                .OrderBy(r => r.Id)
                .Select(r => new ReceivedNonAggressionPactRequestViewModel() {
                    Id = r.Id,
                    AllianceId = r.Sender.Id,
                    Code = r.Sender.Code,
                    Name = r.Sender.Name
                });
        }
    }
}
