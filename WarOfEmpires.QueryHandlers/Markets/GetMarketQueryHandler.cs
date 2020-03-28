using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Markets {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetMarketQueryHandler : IQueryHandler<GetMarketQuery, MarketModel> {

        private readonly IWarContext _context;

        public GetMarketQueryHandler(IWarContext context) {
            _context = context;
        }

        public MarketModel Execute(GetMarketQuery query) {
            var player = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new MarketModel() {
                TotalMerchants = player.GetWorkerCount(WorkerType.Merchants),
                AvailableMerchants = player.GetWorkerCount(WorkerType.Merchants) - player.Caravans.Count,
                CaravanCapacity = player.GetBuildingBonus(BuildingType.Market),
                AvailableCapacity = (player.GetWorkerCount(WorkerType.Merchants) - player.Caravans.Count) * player.GetBuildingBonus(BuildingType.Market),
                FoodInfo = GetMerchandiseInfo(MerchandiseType.Food),
                WoodInfo = GetMerchandiseInfo(MerchandiseType.Wood),
                StoneInfo = GetMerchandiseInfo(MerchandiseType.Stone),
                OreInfo = GetMerchandiseInfo(MerchandiseType.Ore)
            };
        }

        private MerchandiseInfoViewModel GetMerchandiseInfo(MerchandiseType type) {
            var merchandiseInfo = _context.Players
                .Where(p => p.User.Status == UserStatus.Active)
                .SelectMany(p => p.Caravans)
                .SelectMany(c => c.Merchandise)
                .Where(m => m.Type == type)
                .GroupBy(m => m.Price)
                .Select(g => new {
                    Price =  g.Key,
                    Quantity = g.Sum(m => m.Quantity)
                })
                .ToList();
            var minimumMerchandise = merchandiseInfo.OrderBy(m => m.Price).FirstOrDefault();

            return new MerchandiseInfoViewModel() {
                LowestPrice = minimumMerchandise?.Price ?? 0,
                AvailableAtLowestPrice = minimumMerchandise?.Quantity ?? 0,
                TotalAvailable = merchandiseInfo.Sum(m => m.Quantity)
            };
        }
    }
}