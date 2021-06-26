using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WarOfEmpires.TagHelpers {
    public class VictoryStatusTagHelper : PartialTagHelper {
        public VictoryStatusTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) {
            Url = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext).Action("_VictoryStatus", "Player");
            AjaxRefresh = true;
        }
    }
}
