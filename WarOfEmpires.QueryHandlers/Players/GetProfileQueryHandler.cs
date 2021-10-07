using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Configuration;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Players {
    [TransientServiceImplementation(typeof(IQueryHandler<GetProfileQuery, ProfileModel>))]
    public sealed class GetProfileQueryHandler : IQueryHandler<GetProfileQuery, ProfileModel> {
        private readonly IReadOnlyWarContext _context;
        private readonly AppSettings _appSettings;

        public GetProfileQueryHandler(IReadOnlyWarContext context, AppSettings appSettings) {
            _context = context;
            _appSettings = appSettings;
        }

        [Audit]
        public ProfileModel Execute(GetProfileQuery query) {
            return _context.Players
                .Where(p => EmailComparisonService.Equals(p.User.Email, query.Email))
                .Select(p => new ProfileModel() {
                    FullName = p.Profile.FullName,
                    Description = p.Profile.Description,
                    AvatarLocation = string.IsNullOrWhiteSpace(p.Profile.AvatarLocation) ? null : $"{_appSettings.UserImageBaseUrl.TrimEnd('/')}/{p.Profile.AvatarLocation}"
                })
                .Single();
        }
    }
}
