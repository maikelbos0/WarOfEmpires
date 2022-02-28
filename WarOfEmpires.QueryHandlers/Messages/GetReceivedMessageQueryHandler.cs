using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.Utilities.Auditing;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Messages {
    public sealed class GetReceivedMessageQueryHandler : IQueryHandler<GetReceivedMessageQuery, ReceivedMessageDetailsViewModel> {

        private readonly IReadOnlyWarContext _context;

        public GetReceivedMessageQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public ReceivedMessageDetailsViewModel Execute(GetReceivedMessageQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.ReceivedMessages.Select(m => new ReceivedMessageDetailsViewModel() {
                    Id = m.Id,
                    SenderId = m.Sender.User.Status == UserStatus.Active ? m.Sender.Id : default(int?),
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