using WarOfEmpires.Database;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using System;
using System.Linq;

namespace WarOfEmpires.QueryHandlers.Security {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetUserProfileQueryHandler : IQueryHandler<GetUserProfileQuery, UserProfileModel> {
        private readonly IWarContext _context;

        public GetUserProfileQueryHandler(IWarContext context) {
            _context = context;
        }

        public UserProfileModel Execute(GetUserProfileQuery query) {
            return _context.Users
                .Where(u => u.Email.Equals(query.Email, StringComparison.InvariantCultureIgnoreCase))
                .Select(u => new UserProfileModel() {
                    DisplayName = u.DisplayName,
                    Description = u.Description,
                    ShowEmail = u.ShowEmail
                })
                .Single();
        }
    }
}