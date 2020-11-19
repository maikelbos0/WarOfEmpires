using WarOfEmpires.Models.Messages;

namespace WarOfEmpires.Queries.Messages {
    public sealed class GetSentMessageQuery : IQuery<SentMessageDetailsViewModel> {
        public string Email { get; }
        public int MessageId { get; }

        public GetSentMessageQuery(string email, int messageId) {
            Email = email;
            MessageId = messageId;
        }
    }
}