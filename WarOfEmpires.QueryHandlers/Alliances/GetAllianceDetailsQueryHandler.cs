using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [TransientServiceImplementation(typeof(IQueryHandler<GetAllianceDetailsQuery, AllianceDetailsViewModel>))]
    public sealed class GetAllianceDetailsQueryHandler : IQueryHandler<GetAllianceDetailsQuery, AllianceDetailsViewModel> {
        private readonly IWarContext _context;
        private readonly IEnumFormatter _formatter;

        public GetAllianceDetailsQueryHandler(IWarContext context, IEnumFormatter formatter) {
            _formatter = formatter;
            _context = context;
        }

        [Audit]
        public AllianceDetailsViewModel Execute(GetAllianceDetailsQuery query) {
            var nonAggressionPactAlliances = new List<Alliance>();
            var warAlliances = new List<Alliance>();
            var currentAlliance = _context.Players
                .Include(p => p.Alliance)
                .Include(p => p.Alliance.SentNonAggressionPactRequests).ThenInclude(r => r.Recipient)
                .Include(p => p.Alliance.NonAggressionPacts).ThenInclude(n => n.Alliances)
                .Include(p => p.Alliance.Wars).ThenInclude(w => w.Alliances)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance;

            if (currentAlliance != null) {
                nonAggressionPactAlliances.Add(currentAlliance);
                nonAggressionPactAlliances.AddRange(currentAlliance.SentNonAggressionPactRequests.Select(r => r.Recipient));
                nonAggressionPactAlliances.AddRange(currentAlliance.NonAggressionPacts.SelectMany(p => p.Alliances));
                warAlliances.Add(currentAlliance);
                warAlliances.AddRange(currentAlliance.Wars.SelectMany(p => p.Alliances));
            }

            var alliance = _context.Alliances
                .Include(a => a.Leader)
                .Single(a => a.Id == query.Id);
            var members = _context.Players
                .Include(p => p.Workers)
                .Include(p => p.Troops)
                .Where(p => p.User.Status == UserStatus.Active && p.Alliance.Id == query.Id)
                .OrderBy(p => p.Rank)
                .ToList();

            return new AllianceDetailsViewModel() {
                Id = alliance.Id,
                Status = currentAlliance == null ? null : alliance.Id == currentAlliance.Id ? "Mine" : currentAlliance.NonAggressionPacts.Any(p => p.Alliances.Contains(alliance)) ? "Pact" : currentAlliance.Wars.Any(w => w.Alliances.Contains(alliance)) ? "War" : null,
                Code = alliance.Code,
                Name = alliance.Name,
                LeaderId = alliance.Leader.User.Status == UserStatus.Active ? alliance.Leader.Id : default(int?),
                Leader = alliance.Leader.DisplayName,
                Members = members.Select(p => new AllianceMemberViewModel() {
                    Id = p.Id,
                    Rank = p.Rank,
                    DisplayName = p.DisplayName,
                    Title = _formatter.ToString(p.Title),
                    Population = p.Peasants + p.Workers.Sum(w => w.Count) + p.Troops.Sum(t => t.GetTotals())
                }).ToList(),
                CanReceiveNonAggressionPactRequest = !nonAggressionPactAlliances.Contains(alliance),
                CanReceiveWarDeclaration = !warAlliances.Contains(alliance)
            };
        }
    }
}