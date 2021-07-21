using System.Collections.Generic;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.QueryHandlers.Decorators;

namespace WarOfEmpires.QueryHandlers.Security {
    [TransientServiceImplementation(typeof(IQueryHandler<GetUsersQuery, IEnumerable<UserViewModel>>))]
    public sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IEnumerable<UserViewModel>> {
        private readonly IWarContext _context;

        public GetUsersQueryHandler(IWarContext context) {
            _context = context;
        }

        [Audit]
        public IEnumerable<UserViewModel> Execute(GetUsersQuery query) {
            throw new System.NotImplementedException();
        }
    }
}
