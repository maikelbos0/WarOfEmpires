using System;
using System.Collections.Generic;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Messages {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetSentMessagesQueryHandler : IQueryHandler<GetSentMessagesQuery, List<SentMessageDetailsViewModel>> {
        private readonly IWarContext _context;

        public GetSentMessagesQueryHandler(IWarContext context) {
            _context = context;
        }

        public List<SentMessageDetailsViewModel> Execute(GetSentMessagesQuery query) {
            throw new NotImplementedException();
        }
    }
}