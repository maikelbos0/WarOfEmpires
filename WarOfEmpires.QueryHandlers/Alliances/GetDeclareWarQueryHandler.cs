﻿using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;

namespace WarOfEmpires.QueryHandlers.Alliances {
    public sealed class GetDeclareWarQueryHandler : IQueryHandler<GetDeclareWarQuery, DeclareWarModel> {
        private readonly IReadOnlyWarContext _context;

        public GetDeclareWarQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        public DeclareWarModel Execute(GetDeclareWarQuery query) {
            return _context.Alliances.Select(a => new DeclareWarModel() {
                AllianceId = a.Id,
                AllianceCode = a.Code,
                AllianceName = a.Name
            }).Single(a => a.AllianceId == query.AllianceId);
        }
    }
}
