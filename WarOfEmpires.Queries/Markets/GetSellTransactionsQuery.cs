using System.Collections.Generic;
using WarOfEmpires.Models.Markets;

namespace WarOfEmpires.Queries.Markets {
    public sealed class GetSellTransactionsQuery : IQuery<IEnumerable<TransactionViewModel>> {
        public string Email { get; }

        public GetSellTransactionsQuery(string email) {
            Email = email;
        }
    }
}