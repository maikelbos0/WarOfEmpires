using WarOfEmpires.Database;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.Repositories.Alliances {
    [InterfaceInjectable]
    public sealed class AllianceRepository : IAllianceRepository {
        private readonly IWarContext _context;

        public AllianceRepository(IWarContext context) {
            _context = context;
        }

        public void Add(Alliance alliance) {
            _context.Alliances.Add(alliance);
            _context.SaveChanges();
        }

        public void Update() {
            _context.SaveChanges();
        }
    }
}