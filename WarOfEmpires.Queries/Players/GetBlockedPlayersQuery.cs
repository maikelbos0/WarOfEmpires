using System.Collections.Generic;
using WarOfEmpires.Models.Players;

namespace WarOfEmpires.Queries.Players {
    public sealed class GetBlockedPlayersQuery : IQuery<IEnumerable<BlockedPlayerViewModel>> {
        public string Email { get; }

        public GetBlockedPlayersQuery(string email) {
            Email = email;
        }
    }
}
