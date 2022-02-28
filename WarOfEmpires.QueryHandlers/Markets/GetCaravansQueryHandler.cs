using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Models.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.Utilities.Auditing;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Markets {
    public sealed class GetCaravansQueryHandler : IQueryHandler<GetCaravansQuery, IEnumerable<CaravanViewModel>> {

        private readonly IReadOnlyWarContext _context;

        public GetCaravansQueryHandler(IReadOnlyWarContext context) {
            _context = context;
        }

        [Audit]
        public IEnumerable<CaravanViewModel> Execute(GetCaravansQuery query) {
            var player = _context.Players
                .Include(p => p.Caravans).ThenInclude(c => c.Merchandise)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return player.Caravans.Select(c => new CaravanViewModel() {
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
            }).ToList();
        }
    }
}