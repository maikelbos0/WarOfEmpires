using System.Collections.Generic;
using WarOfEmpires.Models.Messages;

namespace WarOfEmpires.Queries.Messages {
    public sealed class GetSentMessagesQuery : IQuery<List<SentMessageDetailsViewModel>> {
        public string Email { get; }

        public GetSentMessagesQuery(string email) {
            Email = email;
        }
    }
}