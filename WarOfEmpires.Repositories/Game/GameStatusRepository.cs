using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Game;

namespace WarOfEmpires.Repositories.Game {
    public sealed class GameStatusRepository : BaseRepository, IGameStatusRepository {
        public GameStatusRepository(IWarContext context) : base(context) { }

        public GameStatus Get() {
            return _context.GameStatus.Single();
        }
    }
}
