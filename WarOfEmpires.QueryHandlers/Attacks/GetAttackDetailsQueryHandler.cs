using WarOfEmpires.Database;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Queries.Attacks;

namespace WarOfEmpires.QueryHandlers.Attacks {
    public sealed class GetAttackDetailsQueryHandler : IQueryHandler<GetAttackDetailsQuery, AttackDetailsViewModel> {
        private readonly IWarContext _context;

        public GetAttackDetailsQueryHandler(IWarContext context) {
            _context = context;
        }

        public AttackDetailsViewModel Execute(GetAttackDetailsQuery query) {
            throw new System.NotImplementedException();
        }
    }
}