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
    public sealed class GetReceivedMessageQueryHandler : IQueryHandler<GetReceivedMessageQuery, ReceivedMessageDetailsViewModel> {

        private readonly IWarContext _context;

        public GetReceivedMessageQueryHandler(IWarContext context) {
            _context = context;
        }

        public ReceivedMessageDetailsViewModel Execute(GetReceivedMessageQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.ReceivedMessages.Select(m => new ReceivedMessageDetailsViewModel() {
                    Id = m.Id,
                    Sender = m.Sender.DisplayName,
                    Date = m.Date,
                    Subject = m.Subject,
                    Body = m.Body,
                    IsRead = m.IsRead
                }))
                .Single(m => m.Id == query.MessageId);
        }
    }
}