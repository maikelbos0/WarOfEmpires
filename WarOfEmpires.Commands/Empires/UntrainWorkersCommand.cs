using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Commands.Empires {
    public sealed class UntrainWorkersCommand : ICommand {
        public string Email { get; }
        public List<WorkerInfo> Workers { get; }

        public UntrainWorkersCommand(string email, IEnumerable<WorkerInfo> workers) {
            Email = email;
            Workers = workers.ToList();
        }
    }
}