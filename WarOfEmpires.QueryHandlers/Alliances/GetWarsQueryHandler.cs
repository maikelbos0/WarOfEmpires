using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [TransientServiceImplementation(typeof(IQueryHandler<GetWarsQuery, IEnumerable<WarViewModel>>))]
    public sealed class GetWarsQueryHandler : IQueryHandler<GetWarsQuery, IEnumerable<WarViewModel>> {
        private readonly IWarContext _context;

        public GetWarsQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public IEnumerable<WarViewModel> Execute(GetWarsQuery query) {
            var alliance = _context.Players
                .Include(p => p.Alliance.Wars).ThenInclude(w => w.Alliances)
                .Include(p => p.Alliance.Wars).ThenInclude(w => w.PeaceDeclarations)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance;

            return alliance
                .Wars
                .OrderBy(p => p.Id)
                .SelectMany(w => w.Alliances.Select(a => new WarViewModel() {
                    Id = w.Id,
                    AllianceId = a.Id,
                    Code = a.Code,
                    Name = a.Name,
                    PeaceDeclared = w.PeaceDeclarations.Any(d => d == alliance)
                }))
                .Where(p => p.AllianceId != alliance.Id);
            throw new System.NotImplementedException();
        }
    }
}
