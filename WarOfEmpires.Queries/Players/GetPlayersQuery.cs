using System.Collections.Generic;
using WarOfEmpires.Models.Players;

namespace WarOfEmpires.Queries.Players {
    public sealed class GetPlayersQuery : IQuery<IEnumerable<PlayerViewModel>> {
        public string Email { get; }
        public string DisplayName { get; }

        public GetPlayersQuery(string email, string displayName) {
            Email = email;
            DisplayName = displayName;
        }
    }
}