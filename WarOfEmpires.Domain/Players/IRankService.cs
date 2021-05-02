using System.Collections.Generic;

namespace WarOfEmpires.Domain.Players {
    public interface IRankService {
        void Update(IEnumerable<Player> players);
    }
}