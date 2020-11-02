using WarOfEmpires.Models.Messages;

namespace WarOfEmpires.Queries.Messages {
    public sealed class GetMessageRecipientQuery : IQuery<MessageModel> {
        public int PlayerId { get; }

        public GetMessageRecipientQuery(int playerId) {
            PlayerId = playerId;
        }
    }
}