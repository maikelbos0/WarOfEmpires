using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Commands.Empires {
    public sealed class UntrainTroopsCommand : ICommand {
        public string Email { get; }
        public List<TroopInfo> Troops { get; }

        public UntrainTroopsCommand(string email, IEnumerable<TroopInfo> troops) {
            Email = email;
            Troops = troops.ToList();
        }
    }
}
