using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Commands.Markets {
    public sealed class BuyResourcesCommand : ICommand {
        public string Email { get; }
        public List<MerchandiseInfo> Merchandise { get; }

        public BuyResourcesCommand(string email, IEnumerable<MerchandiseInfo> merchandise) {
            Email = email;
            Merchandise = merchandise.ToList();
        }
    }
}