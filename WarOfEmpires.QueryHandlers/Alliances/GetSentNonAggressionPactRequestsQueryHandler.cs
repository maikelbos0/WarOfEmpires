﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    public sealed class GetSentNonAggressionPactRequestsQueryHandler : IQueryHandler<GetSentNonAggressionPactRequestsQuery, IEnumerable<SentNonAggressionPactRequestViewModel>> {
        private readonly IReadOnlyWarContext _context;

        public GetSentNonAggressionPactRequestsQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        public IEnumerable<SentNonAggressionPactRequestViewModel> Execute(GetSentNonAggressionPactRequestsQuery query) {
            return _context.Players
                .Include(p => p.Alliance.SentNonAggressionPactRequests).ThenInclude(r => r.Recipient)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Alliance
                .SentNonAggressionPactRequests
                .OrderBy(r => r.Id)
                .Select(r => new SentNonAggressionPactRequestViewModel() {
                    Id = r.Id,
                    AllianceId = r.Recipient.Id,
                    Code = r.Recipient.Code,
                    Name = r.Recipient.Name
                });
        }
    }
}
