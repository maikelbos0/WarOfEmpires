using System.Web.Mvc;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [RoutePrefix("Layout")]
    public class LayoutController : BaseController {
        public LayoutController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [Route("_Menu")]
        public ActionResult _Menu() {
            if (_authenticationService.IsAuthenticated) {
                return PartialView(_messageService.Dispatch(new GetCurrentPlayerQuery(_authenticationService.Identity)));
            }
            else {
                return PartialView(new CurrentPlayerViewModel() {
                    IsAuthenticated = false
                });
            }
        }
    }
}