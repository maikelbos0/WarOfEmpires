using WarOfEmpires.Domain.Auditing;

namespace WarOfEmpires.Repositories.Auditing {
    public interface ICommandExecutionRepository {
        void Add(CommandExecution commandExecution);
    }
}