using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    public sealed class GetResearchQueryHandler : IQueryHandler<GetResearchQuery, ResearchViewModel> {
        private readonly IReadOnlyWarContext _context;
        private readonly IEnumFormatter _formatter;

        public GetResearchQueryHandler(IReadOnlyWarContext context, IEnumFormatter formatter) {
            _context = context;
            _formatter = formatter;
        }

        [Audit]
        public ResearchViewModel Execute(GetResearchQuery query) {
            var researchType = (ResearchType)Enum.Parse(typeof(ResearchType), query.ResearchType);
            var research = _context.Players
                .Include(p => p.Research)
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.Research)
                .SingleOrDefault(r => r.Type == researchType);

            return new ResearchViewModel() {
                ResearchType = researchType.ToString(),
                Name = _formatter.ToString(researchType),
                Description = ResearchDefinitionFactory.Get(researchType).Description,
                Level = research?.Level ?? 0,
                LevelBonus = ResearchDefinitionFactory.Get(researchType).LevelBonus,
                CurrentBonus = (research?.Level ?? 0) * ResearchDefinitionFactory.Get(researchType).LevelBonus
            };
        }
    }
}
