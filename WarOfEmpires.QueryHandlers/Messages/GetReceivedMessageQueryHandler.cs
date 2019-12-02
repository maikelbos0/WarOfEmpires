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
            var messageId = int.Parse(query.MessageId);
            var message = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .ReceivedMessages.Single(m => m.Id == messageId);

            return new ReceivedMessageDetailsViewModel() {
                Sender = message.Sender.DisplayName,
                Date = message.Date,
                Subject = message.Subject,
                Body = message.Body,
                IsRead = message.IsRead
            };
        }
    }
}