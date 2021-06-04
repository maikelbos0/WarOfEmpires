﻿using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [TransientServiceImplementation(typeof(IQueryHandler<GetReceivedInvitesQuery, IEnumerable<ReceivedInviteViewModel>>))]
    public sealed class GetReceivedInvitesQueryHandler : IQueryHandler<GetReceivedInvitesQuery, IEnumerable<ReceivedInviteViewModel>> {
        private readonly IWarContext _context;

        public GetReceivedInvitesQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public IEnumerable<ReceivedInviteViewModel> Execute(GetReceivedInvitesQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .SelectMany(p => p.Invites)
                .OrderBy(i => i.Date)
                .Select(i => new ReceivedInviteViewModel() {
                    Id = i.Id,
                    Date = i.Date,
                    IsRead = i.IsRead,
                    AllianceId = i.Alliance.Id,
                    AllianceCode = i.Alliance.Code,
                    AllianceName = i.Alliance.Name,
                    Subject = i.Subject
                })
                .ToList();
        }
    }
}