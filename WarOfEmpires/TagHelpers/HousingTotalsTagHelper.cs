using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WarOfEmpires.TagHelpers {
    public class HousingTotalsTagHelper : PartialTagHelper {
        public HousingTotalsTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
            : base(urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext).Action("_HousingTotals", "Empire")) {
        }
    }
}
