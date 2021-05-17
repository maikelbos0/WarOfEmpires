using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Messages {
    [TransientServiceImplementation(typeof(IQueryHandler<GetSentMessageQuery, SentMessageDetailsViewModel>))]
    public sealed class GetSentMessageQueryHandler : IQueryHandler<GetSentMessageQuery, SentMessageDetailsViewModel> {
        private readonly IWarContext _context;

        public GetSentMessageQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public SentMessageDetailsViewModel Execute(GetSentMessageQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.SentMessages.Select(m => new SentMessageDetailsViewModel() {
                    Id = m.Id,
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