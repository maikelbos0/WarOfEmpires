using WarOfEmpires.Database;
using WarOfEmpires.Domain.Auditing;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.Repositories.Auditing {
    [InterfaceInjectable]
    public sealed class CommandExecutionRepository : ICommandExecutionRepository {
        private readonly IWarContext _context;

        public CommandExecutionRepository(IWarContext context) {
            _context = context;
        }

        public void Add(CommandExecution commandExecution) {
            _context.CommandExecutions.Add(commandExecution);
            _context.SaveChanges();
        }
    }
}