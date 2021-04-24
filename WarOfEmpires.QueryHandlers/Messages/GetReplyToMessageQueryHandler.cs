using System;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Messages {
    [ScopedServiceImplementation(typeof(IQueryHandler<GetReplyToMessageQuery, MessageModel>))]
    public sealed class GetReplyToMessageQueryHandler : IQueryHandler<GetReplyToMessageQuery, MessageModel> {
        private readonly IWarContext _context;

        public GetReplyToMessageQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public MessageModel Execute(GetReplyToMessageQuery query) {
            var message = _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.ReceivedMessages)
                .Single(m => m.Id == query.MessageId);

            return new MessageModel() {
                RecipientId = message.Sender.Id,
                Recipient = message.Sender.DisplayName,
                Subject = $"Re: {message.Subject}",
                Body = $"{Environment.NewLine}{message.Sender.DisplayName} wrote on {message.Date:yyyy-MM-dd HH:mm}:{Environment.NewLine}{message.Body}"
            };
        }
    }
}