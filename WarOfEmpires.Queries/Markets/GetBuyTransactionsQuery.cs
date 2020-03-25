using System.Collections.Generic;
using WarOfEmpires.Models.Markets;

namespace WarOfEmpires.Queries.Markets {
    public sealed class GetBuyTransactionsQuery : IQuery<List<TransactionViewModel>> {
        public string Email { get; }

        public GetBuyTransactionsQuery(string email) {
            Email = email;
        }
    }
}