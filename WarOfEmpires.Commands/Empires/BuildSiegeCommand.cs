using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Commands.Empires {
    public sealed class BuildSiegeCommand : ICommand {
        public string Email { get; }
        public List<SiegeWeaponInfo> SiegeWeapons { get; }

        public BuildSiegeCommand(string email, IEnumerable<SiegeWeaponInfo> details) {
            Email = email;
            SiegeWeapons = details.ToList();
        }
    }
}