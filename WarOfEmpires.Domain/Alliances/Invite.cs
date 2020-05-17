using System;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Alliances {
    public class Invite : Entity {
        public Alliance Alliance { get; protected set; }
        public Player Player { get; protected set; }
        public DateTime Date { get; protected set; }
        public virtual bool IsRead { get; set; }
        public virtual string Message { get; protected set; }

        protected Invite() {
        }

        public Invite(Alliance alliance, Player player, string message) {
            Alliance = alliance;
            Player = player;
            Message = message;
            Date = DateTime.UtcNow;
            IsRead = false;
        }
    }
}