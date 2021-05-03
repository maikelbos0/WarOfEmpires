using WarOfEmpires.Domain.Common;
using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.QueryHandlers.Common {
    public interface IResourcesMap {
        ResourcesViewModel ToViewModel(Resources resources);
    }
}