using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Players {
    public sealed class GetProfileQueryHandler : IQueryHandler<GetProfileQuery, ProfileModel> {
        private readonly IReadOnlyWarContext _context;

        public GetProfileQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        public ProfileModel Execute(GetProfileQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Select(p => new ProfileModel() {
                    FullName = p.Profile.FullName,
                    Description = p.Profile.Description,
                    AvatarLocation = p.Profile.AvatarLocation
                })
                .Single();
        }
    }
}
