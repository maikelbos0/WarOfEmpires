using System.Collections.Generic;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Markets {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetSellTransactionsQueryHandler : IQueryHandler<GetSellTransactionsQuery, List<TransactionViewModel>> {
        private readonly IWarContext _context;
        private readonly EnumFormatter _formatter;

        public GetSellTransactionsQueryHandler(IWarContext context, EnumFormatter formatter) {
            _context = context;
            _formatter = formatter;
        }

        public List<TransactionViewModel> Execute(GetSellTransactionsQuery query) {
            throw new System.NotImplementedException();
        }
    }
}