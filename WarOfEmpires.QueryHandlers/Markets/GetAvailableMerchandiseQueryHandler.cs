using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Models.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.QueryHandlers.Markets {
    [InterfaceInjectable]
    [Audit]
    public sealed class GetAvailableMerchandiseQueryHandler : IQueryHandler<GetAvailableMerchandiseQuery, AvailableMerchandiseModel> {

        private readonly IWarContext _context;

        public GetAvailableMerchandiseQueryHandler(IWarContext context) {
            _context = context;
        }

        public AvailableMerchandiseModel Execute(GetAvailableMerchandiseQuery query) {
            return new AvailableMerchandiseModel() {
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
                MinimumPrice = minimumMerchandise?.Price ?? 0,
                AvailableAtMinimumPrice = minimumMerchandise?.Quantity ?? 0,
                TotalAvailable = merchandiseInfo.Sum(m => m.Quantity)
            };
        }
    }
}