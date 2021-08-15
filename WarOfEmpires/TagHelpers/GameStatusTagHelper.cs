using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using WarOfEmpires.Controllers;

namespace WarOfEmpires.TagHelpers {
    public class GameStatusTagHelper : PartialTagHelper {
        public GameStatusTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) {
            Url = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext).Action(nameof(GameController._GameStatus), GameController.Route);
            AjaxRefresh = true;
        }
    }
}
