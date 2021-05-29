using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WarOfEmpires.TagHelpers {
    public class ResourceHeaderTagHelper : PartialTagHelper {
        public ResourceHeaderTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) {
            Url = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext).Action("_ResourceHeader", "Empire");
            AjaxRefresh = true;
        }
    }
}
