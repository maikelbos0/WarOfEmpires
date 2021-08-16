using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using WarOfEmpires.Controllers;

namespace WarOfEmpires.TagHelpers {
    public class BuildingTotalsTagHelper : PartialTagHelper {
        public BuildingTotalsTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) : base(urlHelperFactory, actionContextAccessor) {
            Controller = EmpireController.Route;
            Action = nameof(EmpireController._BuildingTotals);
            AjaxRefresh = true;
        }
    }
}
