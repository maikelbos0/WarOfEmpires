using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Commands.Empires {
    public sealed class TrainTroopsCommand : ICommand {
        public string Email { get; }
        public List<TroopInfo> Troops { get; }

        public TrainTroopsCommand(string email, IEnumerable<TroopInfo> troops) {
            Email = email;
            Troops = troops.ToList();
        }
    }
}