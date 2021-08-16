using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using WarOfEmpires.Controllers;

namespace WarOfEmpires.TagHelpers {
    public class GameStatusTagHelper : PartialTagHelper {
        public GameStatusTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) : base(urlHelperFactory, actionContextAccessor) {
            Controller = GameController.Route;
            Action = nameof(GameController._GameStatus);
            AjaxRefresh = true;
        }
    }
}
