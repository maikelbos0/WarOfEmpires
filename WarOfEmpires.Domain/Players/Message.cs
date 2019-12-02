using System;

namespace WarOfEmpires.Domain.Players {
    public class Message : Entity {
        public virtual Player Sender { get; protected set; }
        public virtual Player Recipient { get; protected set; }
        public virtual DateTime Date { get; protected set; }
        public virtual bool IsRead { get; set; }
        public virtual string Subject { get; protected set; }
        public virtual string Body { get; protected set; }

        protected Message() {
        }

        public Message(Player sender, Player recipient, string subject, string body) {
            Sender = sender;
            Recipient = recipient;
            Subject = subject;
            Body = body;
            Date = DateTime.UtcNow;
            IsRead = false;
        }
    }
}