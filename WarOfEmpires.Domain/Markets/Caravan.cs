﻿using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Markets {
    public class Caravan : Entity {
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

        public void Withdraw() {
            throw new NotImplementedException();
        }

        public virtual int Buy(Player buyer, MerchandiseType type, int requestedQuantity) {
            var merchandise = Merchandise.Single(m => m.Type == type);
            var remainder = merchandise.Buy(Player, buyer, requestedQuantity);

            // TODO this will break in EF
            if (merchandise.Quantity == 0) {
                Merchandise.Remove(merchandise);

                if (!Merchandise.Any()) {
                    Player.Caravans.Remove(this);
                }
            }

            return remainder;
        }
    }
}