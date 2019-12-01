using WarOfEmpires.Models.Messages;

namespace WarOfEmpires.Queries.Messages {
    public sealed class GetReceivedMessageQuery : IQuery<ReceivedMessageDetailsViewModel> {
        public string Email { get; }
        public string MessageId { get; }

        public GetReceivedMessageQuery(string email, string messageId) {
            Email = email;
            MessageId = messageId;
        }
    }
}