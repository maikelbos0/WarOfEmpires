using System;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Empires {
    [TransientServiceImplementation(typeof(IQueryHandler<GetResearchQuery, ResearchModel>))]
    public sealed class GetResearchQueryHandler : IQueryHandler<GetResearchQuery, ResearchModel> {
        private readonly IWarContext _context;
        private readonly IEnumFormatter _formatter;

        public GetResearchQueryHandler(IWarContext context, IEnumFormatter formatter) {
            _context = context;
            _formatter = formatter;
        }

        [Audit]
        public ResearchModel Execute(GetResearchQuery query) {
            throw new NotImplementedException();
        }
    }
}
