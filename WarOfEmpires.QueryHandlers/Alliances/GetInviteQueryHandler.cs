﻿using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    public sealed class GetInviteQueryHandler : IQueryHandler<GetInviteQuery, InviteDetailsViewModel> {
        private readonly IReadOnlyWarContext _context;

        public GetInviteQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        public InviteDetailsViewModel Execute(GetInviteQuery query) {
            var invite = _context.Players
                .Where(p => p.Alliance != null && EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.Alliance.Invites)
                .Single(i => i.Id == query.InviteId && i.Player.User.Status == UserStatus.Active);

            return new InviteDetailsViewModel() {
                Id = invite.Id,
                PlayerId = invite.Player.Id,
                PlayerName = invite.Player.DisplayName,
                IsRead = invite.IsRead,
                Date = invite.Date,
                Subject = invite.Subject,
                Body = invite.Body
            };
        }
    }
}