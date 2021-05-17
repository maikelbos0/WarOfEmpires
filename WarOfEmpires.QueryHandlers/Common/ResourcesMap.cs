using VDT.Core.DependencyInjection;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.QueryHandlers.Common {
    [TransientServiceImplementation(typeof(IResourcesMap))]
    public sealed class ResourcesMap : IResourcesMap {
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