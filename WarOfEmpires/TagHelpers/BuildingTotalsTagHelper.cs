using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WarOfEmpires.TagHelpers {
    public class BuildingTotalsTagHelper : PartialTagHelper {
        public BuildingTotalsTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) {
            Url = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext).Action("_BuildingTotals", "Empire");
            AjaxRefresh = true;
        }
    }
}
