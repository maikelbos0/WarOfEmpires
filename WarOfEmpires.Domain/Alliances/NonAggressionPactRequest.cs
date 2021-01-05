namespace WarOfEmpires.Domain.Alliances {
    public class NonAggressionPactRequest : Entity {
        public virtual Alliance Sender { get; protected set; }
        public virtual Alliance Recipient { get; protected set; }

        protected NonAggressionPactRequest() {
        }

        public NonAggressionPactRequest(Alliance sender, Alliance recipient) {
            Sender = sender;
            Recipient = recipient;
            IsAccepted = false;
        }
    }
}
