using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.Repositories.Players {
    [InterfaceInjectable]
    public sealed class PlayerRepository : IPlayerRepository {
        private readonly IWarContext _context;

        public PlayerRepository(IWarContext context) {
            _context = context;
        }

        public Player Get(int id) {
            return _context.Players.Single(p => p.Id == id);
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