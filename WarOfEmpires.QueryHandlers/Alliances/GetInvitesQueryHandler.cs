using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [ScopedServiceImplementation(typeof(IQueryHandler<GetInvitesQuery, IEnumerable<InviteViewModel>>))]
    public sealed class GetInvitesQueryHandler : IQueryHandler<GetInvitesQuery, IEnumerable<InviteViewModel>> {
        private readonly IWarContext _context;

        public GetInvitesQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public IEnumerable<InviteViewModel> Execute(GetInvitesQuery query) {
            return _context.Players
                .Where(p => p.Alliance != null && EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.Alliance.Invites)
                .Where(i => i.Player.User.Status == UserStatus.Active)
                .Select(i => new InviteViewModel() {
                    Id = i.Id,
                    PlayerId = i.Player.Id,
                    PlayerName = i.Player.DisplayName,
                    IsRead = i.IsRead,
                    Date = i.Date,
                    Subject = i.Subject
                })
                .ToList();
        }
    }
}