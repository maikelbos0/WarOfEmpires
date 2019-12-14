using WarOfEmpires.Database;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Queries.Attacks;

namespace WarOfEmpires.QueryHandlers.Attacks {
    public sealed class GetReceivedAttacksQueryHandler : IQueryHandler<GetReceivedAttacksQuery, ReceivedAttackViewModel> {
        private readonly IWarContext _context;

        public GetReceivedAttacksQueryHandler(IWarContext context) {
            _context = context;
        }

        public ReceivedAttackViewModel Execute(GetReceivedAttacksQuery query) {
            throw new System.NotImplementedException();
        }
    }
}