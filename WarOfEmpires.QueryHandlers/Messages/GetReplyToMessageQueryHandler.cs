using System;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Messages {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetReplyToMessageQueryHandler : IQueryHandler<GetReplyToMessageQuery, MessageModel> {
        private readonly IWarContext _context;

        public GetReplyToMessageQueryHandler(IWarContext context) {
            _context = context;
        }

        public MessageModel Execute(GetReplyToMessageQuery query) {
            throw new NotImplementedException();
        }
    }
}