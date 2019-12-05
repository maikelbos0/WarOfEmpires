using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Messages {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetSentMessagesQueryHandler : IQueryHandler<GetSentMessagesQuery, List<SentMessageViewModel>> {
        private readonly IWarContext _context;

        public GetSentMessagesQueryHandler(IWarContext context) {
            _context = context;
        }

        public List<SentMessageViewModel> Execute(GetSentMessagesQuery query) {
            return _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SentMessages
                .Select(m => new SentMessageViewModel() {
                    Id = m.Id,
                    Recipient = m.Recipient.DisplayName,
                    Date = m.Date,
                    IsRead = m.IsRead,
                    Subject = m.Subject
                })
                .ToList();
        }
    }
}