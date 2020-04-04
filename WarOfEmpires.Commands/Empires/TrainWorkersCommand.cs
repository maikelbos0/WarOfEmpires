using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Commands.Empires {
    public sealed class TrainWorkersCommand : ICommand {
        public string Email { get; }
        public List<WorkerInfo> Workers { get; }

        public TrainWorkersCommand(string email, IEnumerable<WorkerInfo> workers) {
            Email = email;
            Workers = workers.ToList();
        }
    }
}