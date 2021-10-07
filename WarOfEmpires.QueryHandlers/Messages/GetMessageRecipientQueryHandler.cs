using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Messages {
    [TransientServiceImplementation(typeof(IQueryHandler<GetMessageRecipientQuery, MessageModel>))]
    public sealed class GetMessageRecipientQueryHandler : IQueryHandler<GetMessageRecipientQuery, MessageModel> {

        private readonly IReadOnlyWarContext _context;

        public GetMessageRecipientQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public MessageModel Execute(GetMessageRecipientQuery query) {
            return _context.Players
                .Select(p => new MessageModel() {
                    RecipientId = p.Id,
                    Recipient = p.DisplayName
                })
                .Single(m => m.RecipientId == query.PlayerId);  
        }
    }
}