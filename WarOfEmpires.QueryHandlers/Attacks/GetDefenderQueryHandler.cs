using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Attacks {
    [ScopedServiceImplementation(typeof(IQueryHandler<GetDefenderQuery, ExecuteAttackModel>))]
    public sealed class GetDefenderQueryHandler : IQueryHandler<GetDefenderQuery, ExecuteAttackModel> {
        private readonly IWarContext _context;

        public GetDefenderQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public ExecuteAttackModel Execute(GetDefenderQuery query) {
            var player = _context.Players
                .Single(p => p.User.Status == UserStatus.Active && p.Id == query.Id);

            return new ExecuteAttackModel() {
                DefenderId = player.Id,
                DisplayName = player.DisplayName,
                Population = player.Peasants + player.Workers.Sum(w => w.Count) + player.Troops.Sum(t => t.GetTotals()),
                Turns = 10
            };
        }
    }
}
