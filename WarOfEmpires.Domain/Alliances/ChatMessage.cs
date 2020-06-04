using System;
using WarOfEmpires.Domain.Players;

namespace WarOfEmpires.Domain.Alliances {
    public class ChatMessage : Entity {
        public virtual Player Player { get; protected set; }
        public virtual DateTime Date { get; protected set; }
        public virtual string Message { get; protected set; }

        protected ChatMessage() {
        }

        public ChatMessage(Player player, string message) {
            Player = player;
            Date = DateTime.UtcNow;
            Message = message;
        }
    }
}