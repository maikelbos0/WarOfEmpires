using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Markets {
    public sealed class GetSellTransactionsQueryHandler : IQueryHandler<GetSellTransactionsQuery, IEnumerable<TransactionViewModel>> {
        private readonly IReadOnlyWarContext _context;
        private readonly IEnumFormatter _formatter;

        public GetSellTransactionsQueryHandler(IReadOnlyWarContext context, IEnumFormatter formatter) {
            _context = context;
            _formatter = formatter;
        }

        public IEnumerable<TransactionViewModel> Execute(GetSellTransactionsQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.SellTransactions)
                .ToList()
                .Select(m => new TransactionViewModel() {
                    Id = m.Id,
                    Date = m.Date,
                    Type = _formatter.ToString(m.Type),
                    Quantity = m.Quantity,
                    Price = m.Price
                })
                .ToList();
        }
    }
}