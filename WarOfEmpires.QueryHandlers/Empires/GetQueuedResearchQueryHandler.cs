using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    [TransientServiceImplementation(typeof(IQueryHandler<GetQueuedResearchQuery, IEnumerable<QueuedResearchViewModel>>))]
    public sealed class GetQueuedResearchQueryHandler : IQueryHandler<GetQueuedResearchQuery, IEnumerable<QueuedResearchViewModel>> {
        private readonly IWarContext _context;
        private readonly IEnumFormatter _formatter;

        public GetQueuedResearchQueryHandler(IWarContext context, IEnumFormatter formatter) {
            _context = context;
            _formatter = formatter;
        }

        [Audit]
        public IEnumerable<QueuedResearchViewModel> Execute(GetQueuedResearchQuery query) {
            var player = _context.Players
                .Include(p => p.Research)
                .Include(p => p.QueuedResearch)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));
            var completedResearch = player.Research.Sum(r => r.Level);
            var queuedResearch = player.QueuedResearch
                .OrderBy(r => r.Priority)
                .Select((r, i) => new { 
                    QueuedResearch = r,
                    Index = i
                })
                .ToList();

            return queuedResearch.Select(r => new QueuedResearchViewModel() {
                Id = r.QueuedResearch.Id,
                Type = r.QueuedResearch.Type.ToString(),
                Name = _formatter.ToString(r.QueuedResearch.Type),
                Priority = r.QueuedResearch.Priority,
                CompletedResearchTime = r.QueuedResearch.CompletedResearchTime,
                NeededResearchTime = ResearchTimeCalculator.GetResearchTime(completedResearch + r.Index, (player.Research.SingleOrDefault(s => s.Type == r.QueuedResearch.Type)?.Level ?? 0) + queuedResearch.Count(q => q.Index < r.Index && q.QueuedResearch.Type == r.QueuedResearch.Type))
            }).ToList();
        }
    }
}
