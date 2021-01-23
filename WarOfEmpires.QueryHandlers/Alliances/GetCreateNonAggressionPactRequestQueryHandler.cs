using WarOfEmpires.Database;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Alliances {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetCreateNonAggressionPactRequestQueryHandler : IQueryHandler<GetCreateNonAggressionPactRequestQuery, CreateNonAggressionPactRequestModel> {
        private readonly IWarContext context;

        public GetCreateNonAggressionPactRequestQueryHandler(IWarContext context) {
            this.context = context;
        }

        public CreateNonAggressionPactRequestModel Execute(GetCreateNonAggressionPactRequestQuery query) {
            throw new System.NotImplementedException();
        }
    }
}
