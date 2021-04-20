using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Markets {
    [ScopedServiceImplementation(typeof(IQueryHandler<GetSellTransactionsQuery, IEnumerable<TransactionViewModel>>))]
    [Audit]
    public sealed class GetSellTransactionsQueryHandler : IQueryHandler<GetSellTransactionsQuery, IEnumerable<TransactionViewModel>> {
        private readonly IWarContext _context;
        private readonly EnumFormatter _formatter;

        public GetSellTransactionsQueryHandler(IWarContext context, EnumFormatter formatter) {
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