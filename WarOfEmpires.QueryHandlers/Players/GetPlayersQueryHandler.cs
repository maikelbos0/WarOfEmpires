using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Players {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetPlayersQueryHandler : IQueryHandler<GetPlayersQuery, List<PlayerViewModel>> {
        private readonly IWarContext _context;

        public GetPlayersQueryHandler(IWarContext context) {
            _context = context;
        }

        public List<PlayerViewModel> Execute(GetPlayersQuery query) {
            return _context.Players
                .Include(p => p.User)
                .Where(p => p.User.Status == UserStatus.Active)
                .OrderBy(u => u.Id)
                .Select(u => new PlayerViewModel() {
                    Id = u.Id,
                    DisplayName = u.DisplayName
                })
                .ToList();
        }
    }
}