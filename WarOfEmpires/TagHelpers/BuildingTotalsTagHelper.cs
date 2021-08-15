using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using WarOfEmpires.Controllers;

namespace WarOfEmpires.TagHelpers {
    public class BuildingTotalsTagHelper : PartialTagHelper {
        public BuildingTotalsTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) {
            Url = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext).Action(nameof(EmpireController._BuildingTotals), EmpireController.Route);
            AjaxRefresh = true;
        }
    }
}
