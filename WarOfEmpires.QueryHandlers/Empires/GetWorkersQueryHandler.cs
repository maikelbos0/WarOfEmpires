using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetWorkersQueryHandler : IQueryHandler<GetWorkersQuery, WorkerModel> {
        public WorkerModel Execute(GetWorkersQuery query) {
            throw new System.NotImplementedException();
        }
    }
}