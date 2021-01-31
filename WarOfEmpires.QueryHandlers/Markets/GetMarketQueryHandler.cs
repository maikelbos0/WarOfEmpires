using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Markets {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetMarketQueryHandler : IQueryHandler<GetMarketQuery, MarketModel> {
        private readonly IWarContext _context;
        private readonly EnumFormatter _formatter;

        public GetMarketQueryHandler(IWarContext context, EnumFormatter formatter) {
            _context = context;
            _formatter = formatter;
        }

        public MarketModel Execute(GetMarketQuery query) {
            var player = _context.Players
                .Include(p => p.Workers)
                .Include(p => p.Buildings)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));
            var merchandise = _context.Players
                .Where(p => p.User.Status == UserStatus.Active)
                .SelectMany(p => p.Caravans)
                .SelectMany(c => c.Merchandise)
                .GroupBy(m => new { m.Type, m.Price })
                .Select(g => new MerchandiseInfo() {
                    Type = g.Key.Type,
                    Price = g.Key.Price,
                    Quantity = g.Sum(m => m.Quantity)
                })
                .ToList();

            return new MarketModel() {
                TotalMerchants = player.GetWorkerCount(WorkerType.Merchants),
                AvailableMerchants = player.GetWorkerCount(WorkerType.Merchants) - player.Caravans.Count,
                CaravanCapacity = player.GetBuildingBonus(BuildingType.Market),
                AvailableCapacity = (player.GetWorkerCount(WorkerType.Merchants) - player.Caravans.Count) * player.GetBuildingBonus(BuildingType.Market),
                Merchandise = new List<MerchandiseModel>() {
                    MapMerchandise(merchandise, MerchandiseType.Food),
                    MapMerchandise(merchandise, MerchandiseType.Wood),
                    MapMerchandise(merchandise, MerchandiseType.Stone),
                    MapMerchandise(merchandise, MerchandiseType.Ore)
                }
            };
        }

        private MerchandiseModel MapMerchandise(IEnumerable<MerchandiseInfo> merchandise, MerchandiseType type) {
            merchandise = merchandise.Where(v => v.Type == type);

            var minimumMerchandise = merchandise.OrderBy(m => m.Price).FirstOrDefault();

            return new MerchandiseModel() {
                Type = type.ToString(),
                Name = _formatter.ToString(type),
                LowestPrice = minimumMerchandise?.Price ?? 0,
                AvailableAtLowestPrice = minimumMerchandise?.Quantity ?? 0,
                TotalAvailable = merchandise.Sum(m => m.Quantity)
            };
        }
    }
}