using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.Repositories.Alliances {
    [InterfaceInjectable]
    public sealed class AllianceRepository : BaseRepository, IAllianceRepository {
        public AllianceRepository(IWarContext context) : base(context) { }

        public Alliance Get(string playerEmail) {
            return _context.Players.Single(p => p.User.Status == UserStatus.Active && EmailComparisonService.Equals(p.User.Email, playerEmail)).Alliance;
        }

        public void Add(Alliance alliance) {
            _context.Alliances.Add(alliance);
            _context.SaveChanges();
        }
    }
}