using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Markets {
    public sealed class Caravan : Entity {
        public Player Player { get; private set; }
        public DateTime Date { get; private set; }
        public ICollection<Merchandise> Merchandise { get; set; } = new List<Merchandise>();

        private Caravan() { }

        public Caravan(Player player) {
            Player = player;
            Date = DateTime.UtcNow;
        }

        public int GetRemainingCapacity(int maximumCapacity) {
            return maximumCapacity - Merchandise.Sum(m => m.Quantity);
        }

        public void Withdraw() {
            throw new NotImplementedException();
        }

        public void Buy(Player buyer, MerchandiseType type, int quantity) {
            var merchandise = Merchandise.Single(m => m.Type == type);

            merchandise.Buy(Player, buyer, quantity);

            // TODO this will break in EF
            if (merchandise.Quantity == 0) {
                Merchandise.Remove(merchandise);

                if (!Merchandise.Any()) {
                    Player.Caravans.Remove(this);
                }
            }            
        }
    }
}