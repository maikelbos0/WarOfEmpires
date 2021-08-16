using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using WarOfEmpires.Controllers;

namespace WarOfEmpires.TagHelpers {
    public class HousingTotalsTagHelper : PartialTagHelper {
        public HousingTotalsTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) : base(urlHelperFactory, actionContextAccessor) {
            Controller = EmpireController.Route;
            Action = nameof(EmpireController._HousingTotals);
            AjaxRefresh = true;
        }
    }
}
