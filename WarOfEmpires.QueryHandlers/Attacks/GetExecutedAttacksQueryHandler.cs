using WarOfEmpires.Database;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Queries.Attacks;

namespace WarOfEmpires.QueryHandlers.Attacks {
    public sealed class GetExecutedAttacksQueryHandler : IQueryHandler<GetExecutedAttacksQuery, ExecutedAttackViewModel> {
        private readonly IWarContext _context;

        public GetExecutedAttacksQueryHandler(IWarContext context) {
            _context = context;
        }

        public ExecutedAttackViewModel Execute(GetExecutedAttacksQuery query) {
            throw new System.NotImplementedException();
        }
    }
}