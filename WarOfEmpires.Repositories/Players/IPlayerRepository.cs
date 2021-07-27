using System.Collections.Generic;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Repositories.Players {
    public interface IPlayerRepository : IBaseRepository {
        void Add(Player player);
        Player Get(string email);
        Player Get(int id);
        Player GetIgnoringStatus(int id);
        IEnumerable<Player> GetAll();
        IEnumerable<Caravan> GetCaravans(MerchandiseType merchandiseType);
    }
}