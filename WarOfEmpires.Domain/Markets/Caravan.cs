using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Markets {
    public class Caravan : Entity {
        public const int MinimumPercentageModifier = 15;
        public const int MaximumPercentageModifier = 5;

        public virtual Player Player { get; protected set; }
        public virtual DateTime Date { get; protected set; }
        public virtual ICollection<Merchandise> Merchandise { get; set; } = new List<Merchandise>();

        protected Caravan() { }

        public Caravan(Player player) {
            Player = player;
            Date = DateTime.UtcNow;
        }

        public int GetRemainingCapacity(int maximumCapacity) {
            return maximumCapacity - Merchandise.Sum(m => m.Quantity);
        }

        public virtual void Withdraw() {
            var hours = Math.Round((DateTime.UtcNow - Date).TotalHours, 3);
            var minimumPercentageSaved = 1.0 / (hours + MinimumPercentageModifier) * MinimumPercentageModifier;
            var maximumPercentageSaved = 1.0 / (hours + MaximumPercentageModifier) * MaximumPercentageModifier;
            var percentageSaved = new Random().NextDouble() * (maximumPercentageSaved - minimumPercentageSaved) + minimumPercentageSaved;

            foreach (var merchandise in Merchandise) {
                merchandise.Withdraw(Player, percentageSaved);
            }

            Player.Caravans.Remove(this);
        }

        public virtual int Buy(Player buyer, MerchandiseType type, int requestedQuantity) {
            var merchandise = Merchandise.Single(m => m.Type == type);
            
            return merchandise.Buy(Player, buyer, requestedQuantity);
        }
    }
}