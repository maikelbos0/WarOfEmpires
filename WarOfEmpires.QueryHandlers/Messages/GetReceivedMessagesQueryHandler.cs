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
    public sealed class GetReceivedMessagesQueryHandler : IQueryHandler<GetReceivedMessagesQuery, List<ReceivedMessageViewModel>> {

        private readonly IWarContext _context;

        public GetReceivedMessagesQueryHandler(IWarContext context) {
            _context = context;
        }

        public List<ReceivedMessageViewModel> Execute(GetReceivedMessagesQuery query) {
            return _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .ReceivedMessages
                .Select(m => new ReceivedMessageViewModel() {
                    Id = m.Id,
                    Sender = m.Sender.DisplayName,
                    Date = m.Date,
                    IsRead = m.IsRead,
                    Subject = m.Subject
                })
                .ToList();
        }
    }
}