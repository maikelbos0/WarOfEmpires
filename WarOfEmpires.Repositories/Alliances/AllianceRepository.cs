using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.Repositories.Alliances {
    [TransientServiceImplementation(typeof(IAllianceRepository))]
    public sealed class AllianceRepository : BaseRepository, IAllianceRepository {
        public AllianceRepository(IWarContext context) : base(context) { }

        public Alliance Get(string playerEmail) {
            return _context.Players.Single(p => p.User.Status == UserStatus.Active && EmailComparisonService.Equals(p.User.Email, playerEmail)).Alliance;
        }

        public Alliance Get(int id) {
            return _context.Alliances.Single(a => a.Id == id);
        }

        public void Add(Alliance alliance) {
            _context.Alliances.Add(alliance);
            _context.SaveChanges();
        }

        public void Remove(Alliance alliance) {
            // Clean up children
            _context.Set<NonAggressionPact>().RemoveRange(alliance.NonAggressionPacts);
            _context.Set<NonAggressionPactRequest>().RemoveRange(alliance.SentNonAggressionPactRequests);
            _context.Set<War>().RemoveRange(alliance.Wars);

            _context.Alliances.Remove(alliance);
            _context.SaveChanges();
        }
    }
}