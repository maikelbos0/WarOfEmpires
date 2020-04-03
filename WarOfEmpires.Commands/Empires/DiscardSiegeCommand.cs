using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Commands.Empires {
    public sealed class DiscardSiegeCommand : ICommand {
        public string Email { get; }
        public List<SiegeWeaponInfo> SiegeWeapons { get; }

        public DiscardSiegeCommand(string email, IEnumerable<SiegeWeaponInfo> siegeWeapons) {
            Email = email;
            SiegeWeapons = siegeWeapons.ToList();
        }
    }
}