using WarOfEmpires.Models.Messages;

namespace WarOfEmpires.Queries.Messages {
    public sealed class GetReceivedMessageQuery : IQuery<ReceivedMessageDetailsViewModel> {
        public string Email { get; }
        public int MessageId { get; }

        public GetReceivedMessageQuery(string email, int messageId) {
            Email = email;
            MessageId = messageId;
        }
    }
}