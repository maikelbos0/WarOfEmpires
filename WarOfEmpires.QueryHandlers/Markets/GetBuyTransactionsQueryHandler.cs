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
    [TransientServiceImplementation(typeof(IQueryHandler<GetBuyTransactionsQuery, IEnumerable<TransactionViewModel>>))]
    public sealed class GetBuyTransactionsQueryHandler : IQueryHandler<GetBuyTransactionsQuery, IEnumerable<TransactionViewModel>> {
        private readonly IWarContext _context;
        private readonly IEnumFormatter _formatter;

        public GetBuyTransactionsQueryHandler(IWarContext context, IEnumFormatter formatter) {
            _context = context;
            _formatter = formatter;
        }

        [Audit]
        public IEnumerable<TransactionViewModel> Execute(GetBuyTransactionsQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.BuyTransactions)
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