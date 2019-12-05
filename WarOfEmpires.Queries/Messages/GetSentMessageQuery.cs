using WarOfEmpires.Models.Messages;

namespace WarOfEmpires.Queries.Messages {
    public sealed class GetSentMessageQuery : IQuery<SentMessageDetailsViewModel> {
        public string Email { get; }
        public string MessageId { get; }

        public GetSentMessageQuery(string email, string messageId) {
            Email = email;
            MessageId = messageId;
        }
    }
}