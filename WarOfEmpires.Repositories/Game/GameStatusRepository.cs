using System.Linq;
using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Game;

namespace WarOfEmpires.Repositories.Game {
    [TransientServiceImplementation(typeof(IGameStatusRepository))]
    public sealed class GameStatusRepository : BaseRepository, IGameStatusRepository {
        public GameStatusRepository(IWarContext context) : base(context) { }

        public GameStatus Get() {
            return _context.GameStatus.Single();
        }
    }
}
