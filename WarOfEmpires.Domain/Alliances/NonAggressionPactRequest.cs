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

        public virtual void Accept() {
            var pact = new NonAggressionPact();

            pact.Alliances.Add(Sender);
            pact.Alliances.Add(Recipient);
            Sender.NonAggressionPacts.Add(pact);
            Recipient.NonAggressionPacts.Add(pact);

            Sender.SentNonAggressionPactRequests.Remove(this);
            Recipient.ReceivedNonAggressionPactRequests.Remove(this);

            var message = $"{Sender.Name} and {Recipient.Name} have entered a non-aggression pact.";
            Sender.PostChatMessage(message);
            Recipient.PostChatMessage(message);
        }

        public virtual void Reject() {
            Sender.SentNonAggressionPactRequests.Remove(this);
            Recipient.ReceivedNonAggressionPactRequests.Remove(this);
        }

        public virtual void Withdraw() {
            Recipient.ReceivedNonAggressionPactRequests.Remove(this);            
        }
    }
}
