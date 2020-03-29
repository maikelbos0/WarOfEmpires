using System.Collections.Generic;
using WarOfEmpires.Models.Messages;

namespace WarOfEmpires.Queries.Messages {
    public sealed class GetReceivedMessagesQuery : IQuery<IEnumerable<ReceivedMessageViewModel>> {
        public string Email { get; }

        public GetReceivedMessagesQuery(string email) {
            Email = email;
        }
    }
}