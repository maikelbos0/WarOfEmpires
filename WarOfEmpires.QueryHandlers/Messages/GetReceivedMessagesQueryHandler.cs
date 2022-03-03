using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Messages {
    public sealed class GetReceivedMessagesQueryHandler : IQueryHandler<GetReceivedMessagesQuery, IEnumerable<ReceivedMessageViewModel>> {

        private readonly IReadOnlyWarContext _context;

        public GetReceivedMessagesQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        public IEnumerable<ReceivedMessageViewModel> Execute(GetReceivedMessagesQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.ReceivedMessages.Select(m => new ReceivedMessageViewModel() {
                    Id = m.Id,
                    Sender = m.Sender.DisplayName,
                    Date = m.Date,
                    IsRead = m.IsRead,
                    Subject = m.Subject
                }))
                .ToList();
        }
    }
}