namespace WarOfEmpires.Domain.Alliances {
    public class NonAggressionPactRequest : Entity {
        public virtual Alliance Sender { get; protected set; }
        public virtual Alliance Recipient { get; protected set; }

        protected NonAggressionPactRequest() {
        }

        public NonAggressionPactRequest(Alliance sender, Alliance recipient) {
            Sender = sender;
            Recipient = recipient;
        }

        public void Accept() {
            var pact = new NonAggressionPact();

            pact.Alliances.Add(Sender);
            pact.Alliances.Add(Recipient);
            Sender.NonAggressionPacts.Add(pact);
            Recipient.NonAggressionPacts.Add(pact);

            Sender.SentNonAggressionPactRequests.Remove(this);
            Recipient.ReceivedNonAggressionPactRequests.Remove(this);
        }
    }
}
