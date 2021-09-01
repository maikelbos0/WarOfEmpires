using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Commands.Markets {
    public sealed class BuyBlackMarketResourcesCommand : ICommand {
        public string Email { get; }
        public List<BlackMarketMerchandiseInfo> Merchandise { get; }

        public BuyBlackMarketResourcesCommand(string email, IEnumerable<BlackMarketMerchandiseInfo> merchandise) {
            Email = email;
            Merchandise = merchandise.ToList();
        }
    }
}