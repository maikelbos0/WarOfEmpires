using WarOfEmpires.Domain.Common;
using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.QueryHandlers.Empires {
    public sealed class ResourcesMap {
        public ResourcesViewModel ToViewModel(Resources resources) {
            return new ResourcesViewModel() {
                Gold = resources.Gold,
                Food = resources.Food,
                Wood = resources.Wood,
                Stone = resources.Stone,
                Ore = resources.Ore
            };
        }
    }
}