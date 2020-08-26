using WarOfEmpires.Domain.Alliances;

namespace WarOfEmpires.Repositories.Alliances {
    public interface IAllianceRepository {
        // TODO get from player email
        void Add(Alliance alliance);
        void Update();
    }
}