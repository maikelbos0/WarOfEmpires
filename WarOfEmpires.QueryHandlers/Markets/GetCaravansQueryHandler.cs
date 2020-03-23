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
    public sealed class GetCaravansQueryHandler : IQueryHandler<GetCaravansQuery, CaravansModel> {

        private readonly IWarContext _context;

        public GetCaravansQueryHandler(IWarContext context) {
            _context = context;
        }

        public CaravansModel Execute(GetCaravansQuery query) {
            var player = _context.Players
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new CaravansModel() {
                TotalMerchants = player.GetWorkerCount(WorkerType.Merchants),
                AvailableMerchants = player.GetWorkerCount(WorkerType.Merchants) - player.Caravans.Count,
                CaravanCapacity = player.GetBuildingBonus(BuildingType.Market),
                AvailableCapacity = (player.GetWorkerCount(WorkerType.Merchants) - player.Caravans.Count) * player.GetBuildingBonus(BuildingType.Market),
                CurrentCaravans = player.Caravans.Select(c => new CaravanViewModel() {
                    Id = c.Id,
                    Date = c.Date,
                    Food = c.Merchandise.SingleOrDefault(m => m.Type == MerchandiseType.Food)?.Quantity ?? 0,
                    FoodPrice = c.Merchandise.SingleOrDefault(m => m.Type == MerchandiseType.Food)?.Price ?? 0,
                    Wood = c.Merchandise.SingleOrDefault(m => m.Type == MerchandiseType.Wood)?.Quantity ?? 0,
                    WoodPrice = c.Merchandise.SingleOrDefault(m => m.Type == MerchandiseType.Wood)?.Price ?? 0,
                    Stone = c.Merchandise.SingleOrDefault(m => m.Type == MerchandiseType.Stone)?.Quantity ?? 0,
                    StonePrice = c.Merchandise.SingleOrDefault(m => m.Type == MerchandiseType.Stone)?.Price ?? 0,
                    Ore = c.Merchandise.SingleOrDefault(m => m.Type == MerchandiseType.Ore)?.Quantity ?? 0,
                    OrePrice = c.Merchandise.SingleOrDefault(m => m.Type == MerchandiseType.Ore)?.Price ?? 0
                }).ToList(),
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
                    Price = g.Key,
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