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
    public sealed class GetSentMessageQueryHandler : IQueryHandler<GetSentMessageQuery, SentMessageDetailsViewModel> {
        private readonly IWarContext _context;

        public GetSentMessageQueryHandler(IWarContext context) {
            _context = context;
        }

        public SentMessageDetailsViewModel Execute(GetSentMessageQuery query) {
            var messageId = int.Parse(query.MessageId);
            var message = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SentMessages.Single(m => m.Id == messageId);

            return new SentMessageDetailsViewModel() {
                Id = message.Id,
                Recipient = message.Recipient.DisplayName,
                Date = message.Date,
                Subject = message.Subject,
                Body = message.Body,
                IsRead = message.IsRead
            };
        }
    }
}