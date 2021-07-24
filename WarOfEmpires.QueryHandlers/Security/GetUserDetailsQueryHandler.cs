using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Security {
    [TransientServiceImplementation(typeof(IQueryHandler<GetUserDetailsQuery, UserDetailsModel>))]
    public sealed class GetUserDetailsQueryHandler : IQueryHandler<GetUserDetailsQuery, UserDetailsModel> {
        private readonly IWarContext _context;
        private readonly IEnumFormatter _formatter;

        public GetUserDetailsQueryHandler(IWarContext context, IEnumFormatter formatter) {
            _formatter = formatter;
            _context = context;
        }

        [Audit]
        public UserDetailsModel Execute(GetUserDetailsQuery query) {
            throw new System.NotImplementedException();
        }
    }
}
