using System.Collections.Generic;
using System.Linq;

namespace WarOfEmpires.Commands.Markets {
    public sealed class SellBlackMarketResourcesCommand : ICommand {
        public string Email { get; }
        public List<BlackMarketMerchandiseInfo> Merchandise { get; }

        public SellBlackMarketResourcesCommand(string email, IEnumerable<BlackMarketMerchandiseInfo> merchandise) {
            Email = email;
            Merchandise = merchandise.ToList();
        }
    }
}