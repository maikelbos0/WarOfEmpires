using WarOfEmpires.Database;
using WarOfEmpires.Domain.Auditing;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.Repositories.Auditing {
    [InterfaceInjectable]
    public sealed class CommandExecutionRepository : BaseRepository, ICommandExecutionRepository {
        public CommandExecutionRepository(IWarContext context) : base(context) { }

        public void Add(CommandExecution commandExecution) {
            _context.CommandExecutions.Add(commandExecution);
            _context.SaveChanges();
        }
    }
}