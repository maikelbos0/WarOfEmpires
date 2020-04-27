using System.Collections.Generic;
using WarOfEmpires.Domain.Alliances;

namespace WarOfEmpires.Repositories.Alliances {
    public interface IAllianceRepository {
        Alliance Get(int id);
        IEnumerable<Alliance> GetAll();
        void Add(Alliance alliance);
        void Update();
    }
}