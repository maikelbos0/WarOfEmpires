using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Messages {
    [ScopedServiceImplementation(typeof(IQueryHandler<GetSentMessagesQuery, IEnumerable<SentMessageViewModel>>))]
    [Audit]
    public sealed class GetSentMessagesQueryHandler : IQueryHandler<GetSentMessagesQuery, IEnumerable<SentMessageViewModel>> {
        private readonly IWarContext _context;

        public GetSentMessagesQueryHandler(IWarContext context) {
            _context = context;
        }

        public IEnumerable<SentMessageViewModel> Execute(GetSentMessagesQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.SentMessages.Select(m => new SentMessageViewModel() {
                    Id = m.Id,
                    Recipient = m.Recipient.DisplayName,
                    Date = m.Date,
                    IsRead = m.IsRead,
                    Subject = m.Subject
                }))
                .ToList();
        }
    }
}