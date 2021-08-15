using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using WarOfEmpires.Controllers;

namespace WarOfEmpires.TagHelpers {
    public class ResourceHeaderTagHelper : PartialTagHelper {
        public ResourceHeaderTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) {
            Url = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext).Action(nameof(EmpireController._ResourceHeader), EmpireController.Route);
            AjaxRefresh = true;
        }
    }
}
