using System;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Messages {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetSentMessageQueryHandler : IQueryHandler<GetSentMessageQuery, SentMessageDetailsViewModel> {
        private readonly IWarContext _context;

        public GetSentMessageQueryHandler(IWarContext context) {
            _context = context;
        }

        public SentMessageDetailsViewModel Execute(GetSentMessageQuery query) {
            throw new NotImplementedException();
        }
    }
}