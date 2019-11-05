using WarOfEmpires.Database;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.QueryHandlers.Security {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IList<UserViewModel>> {
        private readonly IWarContext _context;

        public GetUsersQueryHandler(IWarContext context) {
            _context = context;
        }

        public IList<UserViewModel> Execute(GetUsersQuery query) {
            return _context.Users
                .Where(u => u.Status == UserStatus.Active)
                .OrderBy(u => u.Id)
                .Select(u => new UserViewModel() {
                    Id = u.Id,
                    DisplayName = u.DisplayName ?? "Anonymous",
                    Email = u.ShowEmail ? u.Email : "Private",
                    Description = u.Description,
                    StartDate = u.UserEvents.Min(e => (DateTime?)e.Date)
                })
                .ToList();
        }
    }
}