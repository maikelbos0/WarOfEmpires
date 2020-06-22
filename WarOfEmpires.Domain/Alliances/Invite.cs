using System;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Alliances {
    public class Invite : Entity {
        public virtual Alliance Alliance { get; protected set; }
        public virtual Player Player { get; protected set; }
        public virtual DateTime Date { get; protected set; }
        public virtual bool IsRead { get; set; }
        public virtual string Subject { get; protected set; }
        public virtual string Body { get; protected set; }

        protected Invite() {
        }

        public Invite(Alliance alliance, Player player, string subject, string body) {
            Alliance = alliance;
            Player = player;
            Subject = subject;
            Body = body;
            Date = DateTime.UtcNow;
            IsRead = false;
        }
    }
}