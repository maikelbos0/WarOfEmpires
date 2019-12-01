using WarOfEmpires.Database;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Messages {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetReceivedMessageQueryHandler : IQueryHandler<GetReceivedMessageQuery, ReceivedMessageDetailsViewModel> {

        private readonly IWarContext _context;

        public GetReceivedMessageQueryHandler(IWarContext context) {
            _context = context;
        }

        public ReceivedMessageDetailsViewModel Execute(GetReceivedMessageQuery query) {
            throw new System.NotImplementedException();
        }
    }
}