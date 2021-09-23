using System.Collections.Generic;

namespace WarOfEmpires.Domain.Players {
    public interface IRankService {
        double GetRatio(Player dividendPlayer, Player divisorPlayer);
        void Update(IEnumerable<Player> players);
    }
}