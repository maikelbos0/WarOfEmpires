using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Auditing;

namespace WarOfEmpires.Repositories.Auditing {
    [TransientServiceImplementation(typeof(ICommandExecutionRepository))]
    public sealed class CommandExecutionRepository : BaseRepository, ICommandExecutionRepository {
        public CommandExecutionRepository(ILazyWarContext context) : base(context) { }

        public void Add(CommandExecution commandExecution) {
            _context.CommandExecutions.Add(commandExecution);
            _context.SaveChanges();
        }
    }
}