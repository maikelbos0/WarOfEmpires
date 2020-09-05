using WarOfEmpires.Domain.Auditing;

namespace WarOfEmpires.Repositories.Auditing {
    public interface ICommandExecutionRepository : IBaseRepository {
        void Add(CommandExecution commandExecution);
    }
}