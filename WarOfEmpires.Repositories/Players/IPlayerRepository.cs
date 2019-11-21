using System.Collections.Generic;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Repositories.Players {
    public interface IPlayerRepository {
        void Add(Player player);
        Player Get(string email);
        IEnumerable<Player> GetAll();
        void Update();
    }
}