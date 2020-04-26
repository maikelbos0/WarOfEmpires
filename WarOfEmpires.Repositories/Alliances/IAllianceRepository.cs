using System.Collections.Generic;
using WarOfEmpires.Domain.Alliances;

namespace WarOfEmpires.Repositories.Alliances {
    public interface IAllianceRepository {
        void Add(Alliance alliance);
        Alliance Get(int id);
        IEnumerable<Alliance> GetAll();
        void Update();
    }
}