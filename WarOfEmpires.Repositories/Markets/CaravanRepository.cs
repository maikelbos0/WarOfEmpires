using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Repositories.Markets {
    public class CaravanRepository : ICaravanRepository {
        private readonly IWarContext _context;

        public CaravanRepository(IWarContext context) {
            _context = context;
        }

        public IEnumerable<Caravan> GetForMerchandiseType(MerchandiseType type) {
            return _context.Players
                .Where(p => p.User.Status == UserStatus.Active)
                .SelectMany(p => p.Caravans)
                .SelectMany(c => c.Merchandise, (c, m) => new { Caravan = c, Merchandise = m })
                .Where(c => c.Merchandise.Type == type)
                .OrderBy(c => c.Merchandise.Price)
                .ThenBy(c => c.Caravan.Id)
                .Select(c => c.Caravan)
                .Include(c => c.Merchandise)
                .Include(c => c.Player)
                .ToList();
        }

        public void Update() {
            _context.SaveChanges();
        }
    }
}