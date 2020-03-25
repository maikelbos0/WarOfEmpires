using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

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
            return _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SellTransactions
                .Select(m => new TransactionViewModel() {
                    Id = m.Id,
                    Date = m.Date,
                    Type = _formatter.ToString(m.Type),
                    Quantity = m.Quantity,
                    Price = m.Price,
                    IsRead = m.IsRead
                })
                .ToList();
        }
    }
}