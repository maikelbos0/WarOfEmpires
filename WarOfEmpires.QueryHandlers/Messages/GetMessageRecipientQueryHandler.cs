using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Messages {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetMessageRecipientQueryHandler : IQueryHandler<GetMessageRecipientQuery, MessageModel> {

        private readonly IWarContext _context;

        public GetMessageRecipientQueryHandler(IWarContext context) {
            _context = context;
        }

        public MessageModel Execute(GetMessageRecipientQuery query) {
            var player = _context.Players
                .Single(p => p.Id == query.PlayerId);

            return new MessageModel() {
                RecipientId = query.PlayerId.ToString(),
                Recipient = player.DisplayName
            };
        }
    }
}