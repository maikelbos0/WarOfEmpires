using System.Collections.Generic;
using System.Linq;
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

        public Alliance Get(int id) {
            return _context.Alliances.Single(a => a.IsActive && a.Id == id);
        }

        public IEnumerable<Alliance> GetAll() {
            return _context.Alliances.Where(a => a.IsActive).ToList();
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