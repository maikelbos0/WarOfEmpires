using WarOfEmpires.Models.Messages;

namespace WarOfEmpires.Queries.Messages {
    public sealed class GetReplyToMessageQuery : IQuery<MessageModel> {
        public string Email { get; }
        public int MessageId { get; }

        public GetReplyToMessageQuery(string email, int messageId) {
            Email = email;
            MessageId = messageId;
        }
    }
}