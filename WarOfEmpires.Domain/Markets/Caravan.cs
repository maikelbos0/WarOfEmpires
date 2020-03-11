using System.Collections.Generic;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Markets {
    public sealed class Caravan : Entity {
        // TODO: figure out if player reference is needed here
        public Player Player { get; private set; }
        public ICollection<Merchandise> Merchandise { get; set; } = new List<Merchandise>();

        private Caravan() { }

        public Caravan(Player player) {
            Player = player;
        }
    }
}