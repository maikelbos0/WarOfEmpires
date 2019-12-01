using WarOfEmpires.Models.Messages;

namespace WarOfEmpires.Queries.Messages {
    public sealed class GetMessageRecipientQuery : IQuery<MessageModel> {
        public string PlayerId { get; }

        public GetMessageRecipientQuery(string playerId) {
            PlayerId = playerId;
        }
    }
}