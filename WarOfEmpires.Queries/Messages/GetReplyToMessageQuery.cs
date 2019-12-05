using WarOfEmpires.Models.Messages;

namespace WarOfEmpires.Queries.Messages {
    public sealed class GetReplyToMessageQuery : IQuery<MessageModel> {
        public string Email { get; }
        public string MessageId { get; }

        public GetReplyToMessageQuery(string email, string messageId) {
            Email = email;
            MessageId = messageId;
        }
    }
}