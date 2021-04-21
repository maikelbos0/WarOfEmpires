﻿using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Messages {
    [ScopedServiceImplementation(typeof(IQueryHandler<GetMessageRecipientQuery, MessageModel>))]
    [Audit]
    public sealed class GetMessageRecipientQueryHandler : IQueryHandler<GetMessageRecipientQuery, MessageModel> {

        private readonly IWarContext _context;

        public GetMessageRecipientQueryHandler(IWarContext context) {
            _context = context;
        }

        public MessageModel Execute(GetMessageRecipientQuery query) {
            return _context.Players
                .Select(p => new MessageModel() {
                    RecipientId = p.Id,
                    Recipient = p.DisplayName
                })
                .Single(m => m.RecipientId == query.PlayerId);  
        }
    }
}