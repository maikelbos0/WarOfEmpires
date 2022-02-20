using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Messages {
    public sealed class GetSentMessageQueryHandler : IQueryHandler<GetSentMessageQuery, SentMessageDetailsViewModel> {
        private readonly IReadOnlyWarContext _context;

        public GetSentMessageQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public SentMessageDetailsViewModel Execute(GetSentMessageQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.SentMessages.Select(m => new SentMessageDetailsViewModel() {
                    Id = m.Id,
                    RecipientId = m.Recipient.User.Status == UserStatus.Active ? m.Recipient.Id : default(int?),
                    Recipient = m.Recipient.DisplayName,
                    Date = m.Date,
                    Subject = m.Subject,
                    Body = m.Body,
                    IsRead = m.IsRead
                }))
                .Single(m => m.Id == query.MessageId);
        }
    }
}