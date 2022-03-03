using System.Collections.Generic;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Models.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Markets {
    public sealed class GetBlackMarketQueryHandler : IQueryHandler<GetBlackMarketQuery, BlackMarketModel> {
        private readonly IEnumFormatter _formatter;

        public GetBlackMarketQueryHandler(IEnumFormatter formatter) {
            _formatter = formatter;
        }

        public BlackMarketModel Execute(GetBlackMarketQuery query) {
            return new BlackMarketModel() {
                BuyPrice = Player.BlackMarketBuyPrice,
                SellPrice = Player.BlackMarketSellPrice,
                Merchandise = new List<BlackMarketMerchandiseModel>() { 
                    new BlackMarketMerchandiseModel() { Type = MerchandiseType.Food.ToString(), Name = _formatter.ToString(MerchandiseType.Food) },
                    new BlackMarketMerchandiseModel() { Type = MerchandiseType.Wood.ToString(), Name = _formatter.ToString(MerchandiseType.Wood) },
                    new BlackMarketMerchandiseModel() { Type = MerchandiseType.Stone.ToString(), Name = _formatter.ToString(MerchandiseType.Stone) },
                    new BlackMarketMerchandiseModel() { Type = MerchandiseType.Ore.ToString(), Name = _formatter.ToString(MerchandiseType.Ore) }
                }
            };
        }
    }
}
