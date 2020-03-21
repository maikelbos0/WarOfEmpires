using System.Collections.Generic;
using WarOfEmpires.Domain.Markets;

namespace WarOfEmpires.Repositories.Markets {
    public interface ICaravanRepository {
        IEnumerable<Caravan> GetForMerchandiseType(MerchandiseType type);
        void Delete(Caravan caravan);
        void Update();
    }
}