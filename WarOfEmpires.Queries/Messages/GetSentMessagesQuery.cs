using System.Collections.Generic;
using WarOfEmpires.Models.Messages;

namespace WarOfEmpires.Queries.Messages {
    public sealed class GetSentMessagesQuery : IQuery<List<SentMessageViewModel>> {
        public string Email { get; }

        public GetSentMessagesQuery(string email) {
            Email = email;
        }
    }
}