using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Commands.Empires {
    public sealed class DiscardSiegeCommand : ICommand {
        public string Email { get; }
        public List<SiegeWeaponInfo> SiegeWeapons { get; }

        public DiscardSiegeCommand(string email, IEnumerable<SiegeWeaponInfo> details) {
            Email = email;
            SiegeWeapons = details.ToList();
        }
    }
}