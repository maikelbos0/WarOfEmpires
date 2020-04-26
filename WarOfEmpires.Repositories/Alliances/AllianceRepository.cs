using System.Collections.Generic;
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
            throw new System.NotImplementedException();
        }

        public Alliance Get(int id) {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Alliance> GetAll() {
            throw new System.NotImplementedException();
        }

        public void Update() {
            throw new System.NotImplementedException();
        }
    }
}