using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.Repositories.Players {
    [InterfaceInjectable]
    public sealed class PlayerRepository : IPlayerRepository {
        private readonly IWarContext _context;

        public PlayerRepository(IWarContext context) {
            _context = context;
        }

        public Player Get(string email) {
            return _context.Players.Include(p => p.User).Single(p => p.User.Status == UserStatus.Active && EmailComparisonService.Equals(p.User.Email, email));
        }

        public IEnumerable<Player> GetAll() {
            return _context.Players.Include(p => p.User).Where(p => p.User.Status == UserStatus.Active).ToList();
        }

        public void Add(Player player) {
            _context.Players.Add(player);
            _context.SaveChanges();
        }

        public void Update() {
            _context.SaveChanges();
        }
    }
}