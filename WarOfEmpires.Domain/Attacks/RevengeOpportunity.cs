using System;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Attacks {
    public class RevengeOpportunity : Entity {
        public const int ExpirationHours = 72;

        public virtual Player Target { get; protected set; }
        public virtual DateTime ExpirationDate { get; protected set; }

        protected RevengeOpportunity() {
        }

        public RevengeOpportunity(Player target) {
            Target = target;
            ExpirationDate = DateTime.UtcNow.AddHours(ExpirationHours);
        }
    }
}
