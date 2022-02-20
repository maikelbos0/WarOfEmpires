using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    public sealed class GetAllianceHomeQueryHandler : IQueryHandler<GetAllianceHomeQuery, AllianceHomeModel> {
        private readonly IReadOnlyWarContext _context;

        public GetAllianceHomeQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public AllianceHomeModel Execute(GetAllianceHomeQuery query) {
            var alliance = _context.Players
                .Include(p => p.Alliance.Leader)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance;
            var members = _context.Players
                .Include(p => p.Workers)
                .Include(p => p.Troops)
                .Include(p => p.User)
                .Include(p => p.AllianceRole)
                .Where(p => p.User.Status == UserStatus.Active && p.Alliance.Id == alliance.Id)
                .OrderBy(p => p.Rank)
                .ToList();
            var chatMessageCutoffDate = DateTime.UtcNow.AddDays(-7);
            var chatMessages = _context.Alliances
                .Include(a => a.ChatMessages).ThenInclude(m => m.Player.User)
                .Where(a => a.Id == alliance.Id)
                .SelectMany(a => a.ChatMessages)
                .Where(m => m.Date >= chatMessageCutoffDate)
                .OrderByDescending(m => m.Date)
                .ToList();

            return new AllianceHomeModel() {
                Id = alliance.Id,
                Code = alliance.Code,
                Name = alliance.Name,
                LeaderId = alliance.Leader.User.Status == UserStatus.Active ? alliance.Leader.Id : default(int?),
                Leader = alliance.Leader.DisplayName,
                Members = members.Select(p => new AllianceHomeMemberViewModel() {
                    Id = p.Id,
                    LastOnline = p.User.LastOnline,
                    Rank = p.Rank,
                    DisplayName = p.DisplayName,
                    Role = p.AllianceRole?.Name
                }).ToList(),
                ChatMessages = chatMessages
                    .Select(m => new ChatMessageViewModel() {
                        Id = m.Id,
                        PlayerId = m.Player?.User?.Status == UserStatus.Active ? m.Player.Id : default(int?),
                        Player = m.Player?.DisplayName,
                        Date = m.Date,
                        Message = m.Message
                    })
                    .ToList()
            };
        }
    }
}