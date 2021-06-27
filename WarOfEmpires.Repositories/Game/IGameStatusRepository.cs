using WarOfEmpires.Domain.Game;

namespace WarOfEmpires.Repositories.Game {
    public interface IGameStatusRepository : IBaseRepository {
        GameStatus Get();
    }
}