using WarOfEmpires.Domain.Alliances;

namespace WarOfEmpires.Repositories.Alliances {
    public interface IAllianceRepository {
        Alliance Get(string playerEmail);
        void Add(Alliance alliance);
        void Update();
    }
}