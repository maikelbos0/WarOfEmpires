﻿using Microsoft.EntityFrameworkCore;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [TransientServiceImplementation(typeof(IQueryHandler<GetTransferResourcesQuery, TransferResourcesModel>))]
    public sealed class GetTransferResourcesQueryHandler : IQueryHandler<GetTransferResourcesQuery, TransferResourcesModel> {
        private readonly IWarContext _context;

        public GetTransferResourcesQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public TransferResourcesModel Execute(GetTransferResourcesQuery query) {
            var player = _context.Players
                .Include(p => p.Alliance.Members)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new TransferResourcesModel() {
                Recipients = player.Alliance.Members
                    .Where(p => player != p && p.User.Status == UserStatus.Active)
                    .OrderBy(p => p.DisplayName)
                    .Select(p => new TransferResourcesRecipientViewModel() {
                        Id = p.Id,
                        DisplayName = p.DisplayName
                    })
                    .ToList()
            };
        }
    }
}