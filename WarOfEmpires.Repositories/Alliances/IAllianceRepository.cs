using System.Collections.Generic;
using WarOfEmpires.Domain.Alliances;

namespace WarOfEmpires.Repositories.Alliances {
    public interface IAllianceRepository : IBaseRepository {
        Alliance Get(string playerEmail);
        Alliance Get(int id);
        IEnumerable<Alliance> GetAll();
        void Add(Alliance alliance);
        void Remove(Alliance alliance);
    }
}