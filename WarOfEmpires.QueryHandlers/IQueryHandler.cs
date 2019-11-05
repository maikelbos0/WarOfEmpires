using WarOfEmpires.Queries;

namespace WarOfEmpires.QueryHandlers {
    public interface IQueryHandler<TQuery, TReturnValue> where TQuery : IQuery<TReturnValue> {
        TReturnValue Execute(TQuery query);
    }
}